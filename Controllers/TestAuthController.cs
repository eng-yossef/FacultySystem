using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacultySystem.Controllers
{
    public class TestAuthController : Controller
    {
        // Publicly Accessible Page
        public IActionResult PublicPage()
        {
            return Content("This page is accessible to everyone.");
        }

        // Requires Login
        [Authorize]
        public IActionResult ProtectedPage()
        {
            return Content($"Welcome, {User.Identity.Name}! You have access to this page.");
        }

        // Requires Admin Role
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return Content("Only Admin users can access this page.");
        }
    }
}
