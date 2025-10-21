using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Shared.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.RequiredCourse)
                .OrderBy(c => c.DepartmentCode)
                .ThenBy(c => c.CourseNumber)
                .ToListAsync();
        }

        // GET: api/Courses/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.RequiredCourse)
                .Include(c => c.Classes)
                    .ThenInclude(cl => cl.Instructor)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            return course;
        }
        // GET: api/Courses/1/classes
        [HttpGet("{courseId}/classes")]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByCourse(int courseId)
        {
            var classes = await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.ClassSection)
                .ToListAsync();

            if (!classes.Any())
                return NotFound($"No classes found for course ID {courseId}");

            return Ok(classes);
        }

        // GET: api/Courses/1/available-classes
        [HttpGet("{courseId}/available-classes")]
        public async Task<ActionResult<IEnumerable<Class>>> GetAvailableClassesByCourse(int courseId)
        {
            var availableClasses = await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .Where(c => c.CourseId == courseId && c.Capacity > c.EnrolledCount)
                .OrderBy(c => c.ClassSection)
                .ToListAsync();

            return Ok(availableClasses);
        }
        // GET: api/Courses/department/CS
        [HttpGet("department/{departmentCode}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByDepartment(string departmentCode)
        {
            var courses = await _context.Courses
                .Include(c => c.Prerequisites)
                .Where(c => c.DepartmentCode == departmentCode)
                .OrderBy(c => c.CourseNumber)
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/Courses/level/100
        [HttpGet("level/{level}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByLevel(int level)
        {
            var courses = await _context.Courses
                .Where(c => c.Level == level)
                .OrderBy(c => c.DepartmentCode)
                .ThenBy(c => c.CourseNumber)
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/Courses/1/prerequisites
        [HttpGet("{courseId}/prerequisites")]
        public async Task<ActionResult<IEnumerable<Course>>> GetPrerequisites(int courseId)
        {
            var prerequisites = await _context.Prerequisites
                .Where(p => p.CourseId == courseId)
                .Select(p => p.RequiredCourse)
                .ToListAsync();

            return Ok(prerequisites);
        }

        // GET: api/Courses/1/is-eligible/20240001
        [HttpGet("{courseId}/is-eligible/{registrationNumber}")]
        public async Task<ActionResult<bool>> IsStudentEligibleForCourse(int courseId, int registrationNumber)
        {
            // Get student's completed courses with passing grades
            var studentCompletedCourses = await _context.StudentCourseHistory
                .Where(h => h.StudentRegistrationNumber == registrationNumber &&
                           h.IsCompleted &&
                           h.Grade >= 2.0m)
                .Select(h => h.CourseId)
                .ToListAsync();

            var requiredPrerequisites = await _context.Prerequisites
                .Where(p => p.CourseId == courseId)
                .Select(p => p.RequiredCourseId)
                .ToListAsync();

            return requiredPrerequisites.All(prereqId => studentCompletedCourses.Contains(prereqId));
        }
        // GET: api/Courses/for-student/20240001
        [HttpGet("for-student/{registrationNumber}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesForStudent(int registrationNumber)
        {
           
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);

            if (student == null)
                return NotFound($"Student with registration number {registrationNumber} not found");

            
            var courses = await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.RequiredCourse)
                .Include(c => c.Classes)
                    .ThenInclude(cl => cl.Instructor)
                .Where(c => c.DepartmentCode == student.DepartmentCode) 
                .OrderBy(c => c.Level)
                .ThenBy(c => c.CourseNumber)
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/Courses/for-student/20240001/available-classes
        [HttpGet("for-student/{registrationNumber}/available-classes")]
        public async Task<ActionResult<IEnumerable<Class>>> GetAvailableClassesForStudent(int registrationNumber)
        {
            
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);

            if (student == null)
                return NotFound($"Student with registration number {registrationNumber} not found");

            // Get available classes Ely fiha amakn w nafs el courses bta3t el department bta3o

            var availableClasses = await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .Where(c => c.Course.DepartmentCode == student.DepartmentCode && 
                           c.Capacity > c.EnrolledCount) 
                .OrderBy(c => c.Course.Level)
                .ThenBy(c => c.Course.CourseNumber)
                .ThenBy(c => c.ClassSection)
                .ToListAsync();

            return Ok(availableClasses);
        }

        // GET: api/Courses/for-student/20240001/eligible-courses
        [HttpGet("for-student/{registrationNumber}/eligible-courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetEligibleCoursesForStudent(int registrationNumber)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);

            if (student == null)
                return NotFound($"Student with registration number {registrationNumber} not found");

            // Get student's completed courses with passing grades
            var studentCompletedCourses = await _context.StudentCourseHistory
                .Where(h => h.StudentRegistrationNumber == registrationNumber &&
                           h.IsCompleted &&
                           h.Grade >= 2.0m)
                .Select(h => h.CourseId)
                .ToListAsync();

            // Get all courses in student's department
            var departmentCourses = await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.RequiredCourse)
                .Include(c => c.Classes)
                    .ThenInclude(cl => cl.Instructor)
                .Where(c => c.DepartmentCode == student.DepartmentCode)
                .ToListAsync();

            // Filter courses where student meets prerequisites
            var eligibleCourses = departmentCourses.Where(course =>
            {
                var requiredPrerequisites = course.Prerequisites
                    .Select(p => p.RequiredCourseId)
                    .ToList();

                // If no prerequisites, course is eligible
                if (!requiredPrerequisites.Any()) return true;

                // Check if student has completed all prerequisites
                return requiredPrerequisites.All(prereqId => studentCompletedCourses.Contains(prereqId));
            }).ToList();

            return Ok(eligibleCourses);
        }
    }
}
