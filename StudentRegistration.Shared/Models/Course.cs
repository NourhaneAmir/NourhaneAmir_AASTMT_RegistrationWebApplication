using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required, StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;

        [Required, Range(100, 599)]
        public int CourseNumber { get; set; }

        [Required, StringLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [Required, Range(1, 3)]
        public int CreditHours { get; set; }

        [Required, StringLength(50)]
        public string College { get; set; } = string.Empty;

        [Required, Range(100, 500)]
        public int Level { get; set; } // Which year the course is typically taken

        // Navigation properties
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<Prerequisite> Prerequisites { get; set; } = new List<Prerequisite>();
        public virtual ICollection<StudentCourseHistory> StudentHistory { get; set; } = new List<StudentCourseHistory>();

        // Computed property
        public string FullCourseCode => $"{DepartmentCode}{CourseNumber}"; // {CS101}
    }

}
