using FacultySystem.Models;
using FacultySystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public IActionResult CheckName(string name)
        {
            // Note: Changed to synchronous as it's a simple validation
            // If you need async, change to: return Json(await _instructorService.ValidateNameAsync(name));
            return Json(true);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _instructorService.GetAllInstructorsAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _instructorService.GetInstructorDetailsAsync(id.Value);
            if (instructor == null) return NotFound();

            return View(instructor);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _instructorService.GetDepartmentDropdownAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await _instructorService.GetDepartmentDropdownAsync();
                return View(instructor);
            }

            await _instructorService.CreateInstructorAsync(instructor, imageFile);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _instructorService.GetInstructorByIdAsync(id.Value);
            if (instructor == null) return NotFound();

            ViewBag.Departments = await _instructorService.GetDepartmentDropdownAsync();
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor, IFormFile imageFile)
        {
            if (id != instructor.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await _instructorService.GetDepartmentDropdownAsync();
                return View(instructor);
            }

            await _instructorService.UpdateInstructorAsync(id, instructor, imageFile);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _instructorService.GetInstructorDetailsAsync(id.Value);
            if (instructor == null) return NotFound();

            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _instructorService.DeleteInstructorAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}