using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacultySystem.Models
{
    public class Instructor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Remote(action:"checkName","Instructor",ErrorMessage ="Noooooooooooooooo!")]
        public string Name { get; set; }

        public string Specialization { get; set; }

        // New Property for Image URL
        public string? ImageUrl { get; set; } = "images/photo_2022-12-04_15-52-23_edited.jpg";

        // Foreign Key for Department
        public int? DepartmentId { get; set; }

        // Navigation Property
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        // One-to-Many: Instructor teaches multiple Courses
        public ICollection<Course>? Courses { get; set; }

        // 🔗 One-to-One with ApplicationUser
        //[Required]
        public string? UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
