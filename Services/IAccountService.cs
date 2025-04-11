using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacultySystem.Services
{
    public interface IAccountService
    {
        Task<List<SelectListItem>> GetDepartmentsAsync();
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model, IFormFile imageFile, string webRootPath);
        Task<SignInResult> LoginUserAsync(LoginViewModel model);
        Task SignOutAsync();
    }
}