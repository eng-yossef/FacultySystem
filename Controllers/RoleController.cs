using FacultySystem.Models;
using FacultySystem.Services;
using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _roleService.GetAllRolesAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var result = await _roleService.CreateRoleAsync(roleName);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"Role '{roleName}' created successfully!";
                    return RedirectToAction("Index");
                }
                ViewData["ErrorMessage"] = $"Error creating role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }
            else
            {
                ViewData["ErrorMessage"] = "Role name cannot be empty";
            }
            return View("Create");
        }

        public async Task<IActionResult> Assign()
        {
            return View(await _roleService.GetAssignRoleViewModelAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.AssignRoleToUserAsync(model.UserEmail, model.RoleName);
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

            return View(await _roleService.GetAssignRoleViewModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var result = await _roleService.DeleteRoleAsync(roleName);
                if (result != null && result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"Role '{roleName}' deleted successfully!";
                    return RedirectToAction("Index");
                }
                ViewData["ErrorMessage"] = result == null
                    ? "Role not found"
                    : $"Error deleting role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }
            else
            {
                ViewData["ErrorMessage"] = "Role name cannot be empty";
            }
            return View("Index", await _roleService.GetAllRolesAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            return View(await _roleService.GetRemoveRoleViewModelAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(RemoveRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.RemoveRoleFromUserAsync(model.UserEmail, model.RoleName);
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

            return View(await _roleService.GetRemoveRoleViewModelAsync());
        }

        [HttpGet]
        public async Task<JsonResult> GetUserRoles(string email)
        {
            var roles = await _roleService.GetUserRolesAsync(email);
            return Json(new { success = roles != null, roles });
        }
    }
}