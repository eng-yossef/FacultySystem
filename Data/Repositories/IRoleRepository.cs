using FacultySystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacultySystem.Data.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> RemoveRoleFromUserAsync(ApplicationUser user, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName);
    }
}