using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacultySystem.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int CreditHours { get; set; }

        // Foreign Key for Instructor
        public int? InstructorId { get; set; }

        // Navigation Property
        [ForeignKey("InstructorId")]
        public Instructor? Instructor { get; set; }

        // Many-to-Many relationship with Trainee via CourseResult
        public ICollection<CourseResult>? CourseResults { get; set; }
    }

}