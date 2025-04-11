using FacultySystem.Services;
using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    public class CourseResultController : Controller
    {
        private readonly ICourseResultService _courseResultService;

        public CourseResultController(ICourseResultService courseResultService)
        {
            _courseResultService = courseResultService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(int courseId, int traineeId)
        {
            await _courseResultService.RemoveStudentFromCourseAsync(courseId, traineeId);
            return RedirectToAction(nameof(Details), "Course", new { id = courseId });
        }

        public async Task<IActionResult> EditGrade(int courseId, int traineeId)
        {
            var courseResult = await _courseResultService.GetCourseResultAsync(courseId, traineeId);
            if (courseResult == null)
            {
                return NotFound();
            }
            return View(courseResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(int courseId, int traineeId, float newGrade)
        {
            await _courseResultService.UpdateGradeAsync(courseId, traineeId, newGrade);
            return RedirectToAction(nameof(Details), "Course", new { id = courseId });
        }
    }
}