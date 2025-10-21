using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Shared.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Student>> Login(LoginRequest request)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.RegistrationNumber == request.RegistrationNumber &&
                                         s.PasswordHash == request.Password);

            if (student == null)
                return Unauthorized("Invalid registration number or password");

            student.PasswordHash = string.Empty;

            return Ok(student);
        }

        [HttpGet("current-semester")]
        public async Task<ActionResult<Semester>> GetCurrentSemester()
        {
            var semester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            if (semester == null)
                return NotFound("No current semester found");

            return Ok(semester);
        }
    }

    public class LoginRequest
    {
        public int RegistrationNumber { get; set; }
        public string Password { get; set; } = string.Empty;
    }

}
