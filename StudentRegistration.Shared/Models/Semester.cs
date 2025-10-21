using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        [Required, StringLength(20)]
        public string SemesterName { get; set; } = string.Empty;

        [Required, StringLength(9)]
        public string AcademicYear { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationStart { get; set; }

        [Required]
        public DateTime RegistrationEnd { get; set; }

        public bool IsCurrent { get; set; }

        // Navigation properties
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public virtual ICollection<StudentCourseHistory> CourseHistory { get; set; } = new List<StudentCourseHistory>();


        // Computed property
        public string FullSemesterName => $"{SemesterName} {AcademicYear}"; // {Fall 2025-2026}


        // Business methods
        public bool IsRegistrationOpen()
        {
            //test: new DateTime(2025, 11, 21);
            var now = DateTime.Now;
            return now >= RegistrationStart && now <= RegistrationEnd;
        }

    }

}
