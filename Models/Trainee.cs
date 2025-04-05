using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacultySystem.Models
{
    public class Trainee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        // Foreign Key for Department
        public int? DepartmentId { get; set; }

        // Navigation Property
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        // Many-to-Many relationship with Course via CourseResult
        public ICollection<CourseResult> ? CourseResults { get; set; }
    }

}