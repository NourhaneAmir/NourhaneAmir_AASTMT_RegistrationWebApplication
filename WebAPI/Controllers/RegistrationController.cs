using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Shared.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Registration/available-classes/20240001
        [HttpGet("available-classes/{registrationNumber}")]
        public async Task<ActionResult<IEnumerable<Class>>> GetAvailableClasses(int registrationNumber)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);

            var currentSemester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            if (student == null || currentSemester == null)
                return BadRequest("Student or current semester not found");

            // Ykon fi amakn w ykon b nafs el department bta3 el student
            var availableClasses = await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .Where(c => c.Capacity > c.EnrolledCount &&
                           c.Course.DepartmentCode == student.DepartmentCode) 
                .ToListAsync();

            // Filter El classes ely el student m5lassh ell prerequists bta3ha 
            var eligibleClasses = new List<Class>();
            foreach (var classItem in availableClasses)
            {
                if (await HasCompletedPrerequisites(registrationNumber, classItem.CourseId))
                {
                    eligibleClasses.Add(classItem);
                }
            }

            return Ok(eligibleClasses);
        }

        // GET: api/Registration/registered-classes/20240001
        [HttpGet("registered-classes/{registrationNumber}")]
        public async Task<ActionResult<IEnumerable<Class>>> GetRegisteredClasses(int registrationNumber)
        {
            var currentSemester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            if (currentSemester == null)
                return BadRequest("No current semester");

            var registeredClasses = await _context.Registrations
                .Include(r => r.Class)
                    .ThenInclude(c => c.Course)
                .Include(r => r.Class)
                    .ThenInclude(c => c.Instructor)
                .Where(r => r.StudentRegistrationNumber == registrationNumber &&
                           r.SemesterId == currentSemester.SemesterId &&
                           r.Status == "Registered")
                .Select(r => r.Class)
                .ToListAsync();

            return Ok(registeredClasses);
        }

        // POST: api/Registration/register
        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResult>> RegisterForClass(RegistrationRequest request)
        {
            var result = new RegistrationResult();

            try
            {
                var student = await _context.Students
                    .Include(s => s.Registrations)
                    .FirstOrDefaultAsync(s => s.RegistrationNumber == request.StudentRegistrationNumber);

                var classToRegister = await _context.Classes
                    .Include(c => c.Course)
                    .FirstOrDefaultAsync(c => c.ClassId == request.ClassId);

                var currentSemester = await _context.Semesters
                    .FirstOrDefaultAsync(s => s.IsCurrent);

                
                if (student == null || classToRegister == null || currentSemester == null)
                {
                    result.Errors.Add("Invalid student, class, or semester");
                    return BadRequest(result);
                }

                if (!student.CanRegister(currentSemester))
                {
                    result.Errors.Add("Student cannot register at this time");
                    return BadRequest(result);
                }

                if (!classToRegister.HasAvailableSeats())
                {
                    result.Errors.Add("Class is full");
                    return BadRequest(result);
                }

                // Check credit hours
                var currentCredits = await GetCurrentSemesterCreditHours(request.StudentRegistrationNumber);
                if (currentCredits + classToRegister.Course.CreditHours > 18)
                {
                    result.Errors.Add("Cannot exceed 18 credit hours");
                    return BadRequest(result);
                }

                // Check prerequisites
                if (!await HasCompletedPrerequisites(request.StudentRegistrationNumber, classToRegister.CourseId))
                {
                    result.Errors.Add("Prerequisites not met");
                    return BadRequest(result);
                }

                // Check schedule conflicts
                if (await HasScheduleConflict(request.StudentRegistrationNumber, classToRegister))
                {
                    result.Errors.Add("Schedule conflict detected");
                    return BadRequest(result);
                }

                // Check if already registered
                var existingRegistration = await _context.Registrations
                    .FirstOrDefaultAsync(r => r.StudentRegistrationNumber == request.StudentRegistrationNumber &&
                                             r.ClassId == request.ClassId &&
                                             r.SemesterId == currentSemester.SemesterId);

                if (existingRegistration != null)
                {
                    result.Errors.Add("Already registered for this class");
                    return BadRequest(result);
                }

                // Create registration
                var registration = new Registration
                {
                    StudentRegistrationNumber = request.StudentRegistrationNumber,
                    ClassId = request.ClassId,
                    SemesterId = currentSemester.SemesterId,
                    RegistrationDate = DateTime.UtcNow,
                    Status = "Registered"
                };

                _context.Registrations.Add(registration);
                classToRegister.EnrolledCount++;

                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"Successfully registered for {classToRegister.Course.FullCourseCode}";
                result.Registration = registration;

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Registration failed: {ex.Message}");
                return BadRequest(result);
            }
        }

        // POST: api/Registration/drop
        [HttpPost("drop")]
        public async Task<ActionResult<RegistrationResult>> DropClass(DropRequest request)
        {
            var result = new RegistrationResult();

            try
            {
                var currentSemester = await _context.Semesters
                    .FirstOrDefaultAsync(s => s.IsCurrent);

                if (currentSemester == null)
                {
                    result.Errors.Add("No current semester found");
                    return BadRequest(result);
                }

                var registration = await _context.Registrations
                    .Include(r => r.Class)
                    .ThenInclude(c => c.Course)
                    .FirstOrDefaultAsync(r => r.StudentRegistrationNumber == request.StudentRegistrationNumber &&
                                             r.ClassId == request.ClassId &&
                                             r.SemesterId == currentSemester.SemesterId);

                if (registration == null)
                {
                    result.Errors.Add("Registration not found");
                    return BadRequest(result);
                }

                registration.Status = "Dropped";
                registration.Class.EnrolledCount--;

                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"Successfully dropped {registration.Class.Course.FullCourseCode}";

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Drop failed: {ex.Message}");
                return BadRequest(result);
            }
        }

        // GET: api/Registration/schedule/20240001
        [HttpGet("schedule/{registrationNumber}")]
        public async Task<ActionResult<ScheduleDto>> GetStudentSchedule(int registrationNumber)
        {
            var currentSemester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            if (currentSemester == null)
                return BadRequest("No current semester");

            var registeredClasses = await _context.Registrations
                .Include(r => r.Class)
                    .ThenInclude(c => c.Course)
                .Include(r => r.Class)
                    .ThenInclude(c => c.Instructor)
                .Where(r => r.StudentRegistrationNumber == registrationNumber &&
                           r.SemesterId == currentSemester.SemesterId &&
                           r.Status == "Registered")
                .Select(r => r.Class)
                .ToListAsync();

            var totalCredits = registeredClasses.Sum(c => c.Course.CreditHours);

            var schedule = new ScheduleDto
            {
                Classes = registeredClasses,
                TotalCreditHours = totalCredits,
                CurrentSemester = currentSemester.FullSemesterName
            };

            return Ok(schedule);
        }

        // Check the current semester credit hours for a student 3lshan may5odsh aktar mn 18 hours
        private async Task<int> GetCurrentSemesterCreditHours(int studentRegistrationNumber)
        {
            var currentSemester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            if (currentSemester == null) return 0;

            return await _context.Registrations
                .Include(r => r.Class)
                .ThenInclude(c => c.Course)
                .Where(r => r.StudentRegistrationNumber == studentRegistrationNumber &&
                           r.SemesterId == currentSemester.SemesterId &&
                           r.Status == "Registered")
                .SumAsync(r => r.Class.Course.CreditHours);
        }

        //check if the student has completed all prerequisites for a course
        private async Task<bool> HasCompletedPrerequisites(int studentRegistrationNumber, int courseId)
        {
            var prerequisites = await _context.Prerequisites
                .Where(p => p.CourseId == courseId)
                .Select(p => p.RequiredCourseId)
                .ToListAsync();

            if (!prerequisites.Any()) return true; // Malosh prerequisite
            
            var completedCourses = await _context.StudentCourseHistory
                .Where(h => h.StudentRegistrationNumber == studentRegistrationNumber &&
                           h.IsCompleted &&
                           h.Grade >= 2.0m) 
                .Select(h => h.CourseId)
                .ToListAsync();

            return prerequisites.All(prereqId => completedCourses.Contains(prereqId));
        }

        // check law fi schedule conflict ben el class el gdeda wel classes el motasajela feha
        private async Task<bool> HasScheduleConflict(int studentRegistrationNumber, Class newClass)
        {
            var currentRegistrations = await GetRegisteredClasses(studentRegistrationNumber);

            if (currentRegistrations.Value == null) return false;

            return currentRegistrations.Value.Any(existingClass =>
                existingClass.DaysOfWeek == newClass.DaysOfWeek &&
                existingClass.StartTime < newClass.EndTime &&
                existingClass.EndTime > newClass.StartTime);
        }
    }

    public class RegistrationRequest
    {
        public int StudentRegistrationNumber { get; set; }
        public int ClassId { get; set; }
    }

    public class DropRequest
    {
        public int StudentRegistrationNumber { get; set; }
        public int ClassId { get; set; }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public Registration? Registration { get; set; }
    }

    public class ScheduleDto
    {
        public List<Class> Classes { get; set; } = new List<Class>();
        public int TotalCreditHours { get; set; }
        public string CurrentSemester { get; set; } = string.Empty;
    }
}
