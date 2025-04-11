using FacultySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacultySystem.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<List<SelectListItem>> GetDepartmentsAsync();
        Task<string> SaveImageAsync(IFormFile imageFile, string webRootPath);
    }
}

namespace FacultySystem.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}