using FacultySystem.Models;
using FacultySystem.Services;
using FacultySystem.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    [CustomActionFilter]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _courseService.GetAllCoursesAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var course = await _courseService.GetCourseDetailsAsync(id.Value);
            if (course == null) return NotFound();

            return View(course);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Instructors = await _courseService.GetAllInstructorsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                await _courseService.CreateCourseAsync(course);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Instructors = await _courseService.GetAllInstructorsAsync();
            return View(course);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _courseService.GetCourseByIdAsync(id.Value);
            if (course == null) return NotFound();

            ViewBag.Instructors = await _courseService.GetAllInstructorsAsync();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _courseService.UpdateCourseAsync(course);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Instructors = await _courseService.GetAllInstructorsAsync();
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _courseService.GetCourseDetailsAsync(id.Value);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseService.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddStudent(int? id)
        {
            if (id == null) return NotFound();

            var course = await _courseService.GetCourseDetailsAsync(id.Value);
            if (course == null) return NotFound();

            ViewBag.Trainees = await _courseService.GetAvailableTraineesAsync(id.Value);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(int id, int traineeId)
        {
            await _courseService.AddTraineeToCourseAsync(id, traineeId);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(int courseId, int traineeId)
        {
            await _courseService.RemoveTraineeFromCourseAsync(courseId, traineeId);
            return RedirectToAction(nameof(Details), new { id = courseId });
        }
    }
}