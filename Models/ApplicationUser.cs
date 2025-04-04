using Microsoft.AspNetCore.Identity;

namespace FacultySystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional fields like FullName, ProfileImage etc. if needed
        public string? FullName { get; set; }
    }
}
