using FacultySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace FacultySystem.Controllers
{
    public class CourseResultController : Controller
    {
        private readonly FacultyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CourseResultController(FacultyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        //delete student from course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(int courseId, int traineeId)
        {
            var courseResult = await _context.CourseResults
                .FirstOrDefaultAsync(cr => cr.CourseId == courseId && cr.TraineeId == traineeId);
            if (courseResult != null)
            {
                _context.CourseResults.Remove(courseResult);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), "Course", new { id = courseId });
        }

        //Edit Grade

        public IActionResult EditGrade(int courseId, int traineeId)
        {
            var courseResult = _context.CourseResults.Include(cr => cr.Course).Include(cr => cr.Trainee)
                .FirstOrDefault(cr => cr.CourseId == courseId && cr.TraineeId == traineeId);
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
            var courseResult = await _context.CourseResults
                .FirstOrDefaultAsync(cr => cr.CourseId == courseId && cr.TraineeId == traineeId);
            if (courseResult != null)
            {
                courseResult.Grade = newGrade;
                _context.Update(courseResult);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), "Course", new { id = courseId });
        }
    }
}
