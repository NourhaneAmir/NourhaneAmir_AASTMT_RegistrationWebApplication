using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Shared.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemestersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SemestersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Semesters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semester>>> GetSemesters()
        {
            return await _context.Semesters
                .OrderByDescending(s => s.AcademicYear)
                .ThenByDescending(s => s.SemesterName)
                .ToListAsync();
        }

        // GET: api/Semesters/current
        [HttpGet("current")]
        public async Task<ActionResult<Semester>> GetCurrentSemester()
        {
            var semester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            if (semester == null)
                return NotFound();

            return semester;
        }

        // GET: api/Semesters/registration-status
        [HttpGet("registration-status")]
        public async Task<ActionResult<bool>> GetRegistrationStatus()
        {
            var currentSemester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.IsCurrent);

            return currentSemester?.IsRegistrationOpen() ?? false;
        }
    }
}
