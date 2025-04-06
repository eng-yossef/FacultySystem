using FacultySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        // GET: Role/Index
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        // Add a new role
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    ViewData["SuccessMessage"] = $"Role '{roleName}' created successfully!";
                    return RedirectToAction("Index");
                }
                ViewData["ErrorMessage"] = $"Error creating role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }
            else
            {
                ViewData["ErrorMessage"] = "Role name cannot be empty";
            }
            return View();
        }

        // Assign Role to User
        public async Task<IActionResult> Assign()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new AssignRoleViewModel
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

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View(await PopulateViewModel());
                }

                // Remove existing roles first (optional)
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                // Add new role
                var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"Role '{model.RoleName}' assigned to {model.UserEmail} successfully";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(await PopulateViewModel());
        }

        private async Task<AssignRoleViewModel> PopulateViewModel()
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


        //Remove Role 
        [HttpPost]
        public async Task<IActionResult> Delete(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        ViewData["SuccessMessage"] = $"Role '{roleName}' deleted successfully!";
                        return RedirectToAction("Index");
                    }
                    ViewData["ErrorMessage"] = $"Error deleting role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                }
                else
                {
                    ViewData["ErrorMessage"] = "Role not found";
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Role name cannot be empty";
            }
            return View();
        }









        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            return View(await PopulateRemoveViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(RemoveRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View(await PopulateRemoveViewModel());
                }

                // Check if user has the role
                if (!await _userManager.IsInRoleAsync(user, model.RoleName))
                {
                    ModelState.AddModelError("", $"User doesn't have the '{model.RoleName}' role");
                    return View(await PopulateRemoveViewModel());
                }

                var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"Role '{model.RoleName}' removed from {model.UserEmail} successfully";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(await PopulateRemoveViewModel());
        }

        private async Task<RemoveRoleViewModel> PopulateRemoveViewModel()
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

        // Helper method to get roles for a specific user (for AJAX implementation)
        [HttpGet]
        public async Task<JsonResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Json(new { success = true, roles });
        }


    }
}
