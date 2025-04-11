using FacultySystem.Models;
using FacultySystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    [Authorize]
    public class TraineeController : Controller
    {
        private readonly ITraineeService _traineeService;

        public TraineeController(ITraineeService traineeService)
        {
            _traineeService = traineeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _traineeService.GetAllTraineesAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _traineeService.GetTraineeDetailsAsync(id.Value);
            if (trainee == null) return NotFound();

            return View(trainee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _traineeService.GetDepartmentDropdownAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                await _traineeService.CreateTraineeAsync(trainee);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await _traineeService.GetDepartmentDropdownAsync();
            return View(trainee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _traineeService.GetTraineeByIdAsync(id.Value);
            if (trainee == null) return NotFound();

            ViewBag.Departments = await _traineeService.GetDepartmentDropdownAsync();
            return View(trainee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Trainee trainee)
        {
            if (id != trainee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _traineeService.UpdateTraineeAsync(trainee);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await _traineeService.GetDepartmentDropdownAsync();
            return View(trainee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _traineeService.GetTraineeDetailsAsync(id.Value);
            if (trainee == null) return NotFound();

            return View(trainee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _traineeService.DeleteTraineeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _traineeService.GetTraineeDetailsAsync(id.Value);
            if (trainee == null) return NotFound();

            ViewBag.Courses = await _traineeService.GetAvailableCoursesAsync(id.Value);
            return View(trainee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Enroll(int id, int courseId)
        {
            await _traineeService.EnrollInCourseAsync(id, courseId);
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}