using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultySystem.Models;
using System.Linq;
using System.Threading.Tasks;
using FacultySystem.Filters;
using Microsoft.AspNetCore.Authorization;

namespace FacultySystem.Controllers
{
    [CustomActionFilter]
    [Authorize(Roles = "Admin")]

    public class CourseController : Controller
    {

        private readonly FacultyDbContext _context;

        public CourseController(FacultyDbContext context)
        {
            _context = context;
        }

        // GET: Course (List all courses)
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.Include(c => c.Instructor).ToListAsync();
            return View(courses);
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.CourseResults)
                .ThenInclude(cr => cr.Trainee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            ViewBag.Instructors = _context.Instructors.ToList();
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            ViewBag.Instructors = _context.Instructors.ToList();
            return View(course);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //add student to course
        public async Task<IActionResult> AddStudent(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.CourseResults)
                .ThenInclude(cr => cr.Trainee)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            // Get trainees who are NOT in this course
            var enrolledTraineeIds = course.CourseResults.Select(cr => cr.TraineeId).ToList();
            var availableTrainees = _context.Trainees.Where(t => !enrolledTraineeIds.Contains(t.Id)).ToList();

            ViewBag.Trainees = availableTrainees;
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(int id, int traineeId)
        {
            var course = await _context.Courses.FindAsync(id);
            var trainee = await _context.Trainees.FindAsync(traineeId);
            if (course == null || trainee == null) return NotFound();
            var courseResult = new CourseResult { CourseId = id, TraineeId = traineeId };
            _context.CourseResults.Add(courseResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
