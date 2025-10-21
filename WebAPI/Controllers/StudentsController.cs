using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Shared.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Students/{Registration number}
        [HttpGet("{registrationNumber}")]
        public async Task<ActionResult<Student>> GetStudent(int registrationNumber)
        {
            var student = await _context.Students
                .Include(s => s.CourseHistory)
                    .ThenInclude(h => h.Course)
                .Include(s => s.CourseHistory)
                    .ThenInclude(h => h.Semester)
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Class)
                    .ThenInclude(c => c.Course)
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Class)
                    .ThenInclude(c => c.Instructor)
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Semester)
                .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);

            if (student == null)
                return NotFound();

            student.PasswordHash = string.Empty;

            return student;
        }

        // GET: api/Students/{Registration number}/can-register
        [HttpGet("{registrationNumber}/can-register")]
        public async Task<ActionResult<bool>> CanStudentRegister(int registrationNumber)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);

            var currentSemester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            if (student == null || currentSemester == null)
                return false;

            return student.CanRegister(currentSemester);
        }

        // GET: api/Students/{Registration number}/course-history
        [HttpGet("{registrationNumber}/course-history")]
        public async Task<ActionResult<IEnumerable<StudentCourseHistory>>> GetStudentCourseHistory(int registrationNumber)
        {
            var history = await _context.StudentCourseHistory
                .Include(h => h.Course)
                .Include(h => h.Semester)
                .Where(h => h.StudentRegistrationNumber == registrationNumber)
                .OrderByDescending(h => h.Semester.AcademicYear)
                .ThenByDescending(h => h.Semester.SemesterName)
                .ToListAsync();

            return Ok(history);
        }
    }
}
