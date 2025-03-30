using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacultySystem.Models
{
    public class Department
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int Id { get; set; }
        public string Name { get; set; }

        // One-to-Many relationship with Instructor
        public ICollection<Instructor>? Instructors { get; set; }

        // One-to-Many relationship with Trainee
        public ICollection<Trainee>? Trainees { get; set; }
    }

}