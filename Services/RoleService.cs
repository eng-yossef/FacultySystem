using FacultySystem.Models;
using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role != null ? await _roleManager.DeleteAsync(role) : null;
        }

        public async Task<AssignRoleViewModel> GetAssignRoleViewModelAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            return new AssignRoleViewModel
            {
                Users = users.Select(u => new SelectListItem
                {
                    Value = u.Email,
                    Text = u.Email
                }).ToList(),

                Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList()
            };
        }

        public async Task<IdentityResult> AssignRoleToUserAsync(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            // Remove existing roles first
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Add new role
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<RemoveRoleViewModel> GetRemoveRoleViewModelAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            return new RemoveRoleViewModel
            {
                Users = users.Select(u => new SelectListItem
                {
                    Value = u.Email,
                    Text = u.Email
                }).ToList(),

                Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList()
            };
        }

        public async Task<IdentityResult> RemoveRoleFromUserAsync(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (!await _userManager.IsInRoleAsync(user, roleName))
                return IdentityResult.Failed(new IdentityError { Description = $"User doesn't have the '{roleName}' role" });

            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null ? await _userManager.GetRolesAsync(user) : Enumerable.Empty<string>();
        }
    }
}