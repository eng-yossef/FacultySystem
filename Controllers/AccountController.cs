using FacultySystem.Services;
using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FacultySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(
            IAccountService accountService,
            IWebHostEnvironment webHostEnvironment)
        {
            _accountService = accountService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Departments = await _accountService.GetDepartmentsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await _accountService.GetDepartmentsAsync();
                return View(model);
            }

            var result = await _accountService.RegisterUserAsync(model, imageFile, _webHostEnvironment.WebRootPath);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Departments = await _accountService.GetDepartmentsAsync();
                return View(model);
            }

            return model.Role switch
            {
                "Instructor" => RedirectToAction("Index", "Home"),
                "Trainee" => RedirectToAction("Index", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountService.LoginUserAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account locked out");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Login not allowed (confirm email?)");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}