using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.Shared.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorId { get; set; }

        [Required, StringLength(100)]
        public string InstructorName { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;


        // Navigation properties
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }

}
