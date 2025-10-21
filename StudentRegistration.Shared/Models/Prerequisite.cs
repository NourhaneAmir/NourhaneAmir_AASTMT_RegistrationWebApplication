using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class Prerequisite
    {
        [Key]
        public int PrerequisiteId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int RequiredCourseId { get; set; }

        // Navigation properties
        public virtual Course Course { get; set; } = null!;
        public virtual Course RequiredCourse { get; set; } = null!;
    }
}
