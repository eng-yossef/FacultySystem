using FacultySystem.Models;
using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly FacultyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            FacultyDbContext context,
            IWebHostEnvironment webHostEnvironment
         )     
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Departments = await _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToListAsync();

            return View();
        }
        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate departments dropdown if validation fails
                ViewBag.Departments = await GetDepartments();
                return View(model);
            }

            // Create the base user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Departments = await GetDepartments();
                return View(model);
            }

            // Add user to role
            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Departments = await GetDepartments();
                return View(model);
            }

            // Handle role-specific data
            if (model.Role == "Instructor")
            {
                var instructor = new Instructor
                {
                    Name = model.FullName,
                    Specialization = model.Specialization,
                    DepartmentId = model.DepartmentId,
                    UserId = user.Id
                };

                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    instructor.ImageUrl = await SaveImage(imageFile);
                }

                _context.Instructors.Add(instructor);
            }
            else if (model.Role == "Trainee")
            {
                var trainee = new Trainee
                {
                    Name = model.FullName,
                    Age = model.Age, // We know Age has value because of validation
                    DepartmentId = model.DepartmentId,
                    UserId = user.Id
                };

                _context.Trainees.Add(trainee);
            }

            await _context.SaveChangesAsync();

            // Sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Redirect based on role
            return model.Role switch
            {
                "Instructor" => RedirectToAction("Index", "Home"),
                "Trainee" => RedirectToAction("Index", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        private async Task<List<SelectListItem>> GetDepartments()
        {
            return await _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
            .ToListAsync();
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

              return "/images/" + uniqueFileName;
            }

            return null;
        }

        // GET: Login
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }

            // Debugging: Check password separately
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                ModelState.AddModelError("", "Incorrect password");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // Specific error handling
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

        // POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
        
    }
}
