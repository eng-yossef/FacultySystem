using FacultySystem.Models;
using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<AssignRoleViewModel> GetAssignRoleViewModelAsync();
        Task<IdentityResult> AssignRoleToUserAsync(string userEmail, string roleName);
        Task<RemoveRoleViewModel> GetRemoveRoleViewModelAsync();
        Task<IdentityResult> RemoveRoleFromUserAsync(string userEmail, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(string email);
    }
}