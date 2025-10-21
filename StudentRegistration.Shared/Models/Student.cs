using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class Student
    {
        [Key]
        public int RegistrationNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string College { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public int AdmissionYear { get; set; } = 0;

        public int TotalCreditHours { get; set; }

        [Range(0, 4)]
        public decimal GPA { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "Regular"; // {Regular, Graduated, Expelled}



        // Navigation properties
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public virtual ICollection<StudentCourseHistory> CourseHistory { get; set; } = new List<StudentCourseHistory>();



        // Business methods
        public bool CanRegister(Semester semester)
        {
            return Status == "Regular" &&
                   semester.IsRegistrationOpen() &&
                   TotalCreditHours < 132; 
        }

        public bool IsRegularStudent() => Status == "Regular";
    }

}
