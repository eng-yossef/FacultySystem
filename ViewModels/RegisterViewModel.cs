using System.ComponentModel.DataAnnotations;

namespace FacultySystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string? Specialization { get; set; } = string.Empty;
        
        //public string? ImageUrl { get; set; } = string.Empty;

        public int Age { get; set; } = 0;

        public int DepartmentId { get; set; } = 0;


    }
}
