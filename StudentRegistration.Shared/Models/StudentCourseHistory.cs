using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class StudentCourseHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int StudentRegistrationNumber { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        [Range(0, 4)]
        public decimal Grade { get; set; }

        public bool IsCompleted { get; set; }

        // Navigation properties
        public virtual Student Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;

        // Business methods
        public bool IsPassingGrade() => Grade >= 2.0m;
    }

}
