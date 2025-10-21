using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required, StringLength(5)]
        public string ClassSection { get; set; } = string.Empty; 

        [Required, Range(1, 100)]
        public int Capacity { get; set; }

        public int EnrolledCount { get; set; }

        [Required, StringLength(10)]
        public string DaysOfWeek { get; set; } = string.Empty; 

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [StringLength(20)]
        public string Classroom { get; set; } = string.Empty;

        [NotMapped]
        public int RemainingSeats => Capacity - EnrolledCount;


        // Navigation properties
        public virtual Course Course { get; set; } = null!;
        public virtual Instructor Instructor { get; set; } = null!;
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        // Business methods
        public bool HasAvailableSeats() => EnrolledCount < Capacity;
        public int GetRemainingSeats() => Capacity - EnrolledCount;
    }

}
