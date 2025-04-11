using FacultySystem.Models;
using FacultySystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacultySystem.Controllers
{
    [Authorize(Roles = "Admin,Instructor,Trainee")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _departmentService.GetAllDepartmentsAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var department = await _departmentService.GetDepartmentDetailsAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.CreateDepartmentAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddInstructor(int departmentId)
        {
            var department = _departmentService.GetDepartmentByIdAsync(departmentId).Result;
            if (department == null) return NotFound();

            var instructors = _departmentService.GetAvailableInstructorsAsync().Result;

            ViewBag.DepartmentName = department.Name;
            ViewBag.DepartmentId = departmentId;

            return View(instructors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddInstructor(int departmentId, int instructorId)
        {
            await _departmentService.AddInstructorToDepartmentAsync(departmentId, instructorId);
            return RedirectToAction(nameof(Details), new { id = departmentId });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddTrainee(int departmentId)
        {
            var department = _departmentService.GetDepartmentByIdAsync(departmentId).Result;
            if (department == null) return NotFound();

            var trainees = _departmentService.GetAvailableTraineesAsync().Result;

            ViewBag.DepartmentName = department.Name;
            ViewBag.DepartmentId = departmentId;

            return View(trainees);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTrainee(int departmentId, int traineeId)
        {
            await _departmentService.AddTraineeToDepartmentAsync(departmentId, traineeId);
            return RedirectToAction(nameof(Details), new { id = departmentId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentService.UpdateDepartmentAsync(department);
                }
                catch
                {
                    if (await _departmentService.GetDepartmentByIdAsync(id) == null)
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}