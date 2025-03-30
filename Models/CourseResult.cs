using System.ComponentModel.DataAnnotations.Schema;

namespace FacultySystem.Models
{
    public class CourseResult
    {
        public int TraineeId { get; set; }
        public Trainee Trainee { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public float Grade { get; set; }
    }

}