using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        [Required]
        public int StudentRegistrationNumber { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required, StringLength(20)]
        public string Status { get; set; } = "Registered"; // {Registered, Waitlisted => Law madfa3sh masarif masln, Dropped => Law ma7darsh aw el mada etsa7bt mno}

        // Navigation properties
        public virtual Student Student { get; set; } = null!;
        public virtual Class Class { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;

        // Business methods
        public bool IsActive() => Status == "Registered";
        public bool IsWaitlisted() => Status == "Waitlisted";
        public bool IsDropped() => Status == "Dropped";
    }
}
