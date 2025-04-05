using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultySystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacultySystem.Controllers
{
    public class TraineeController : Controller
    {
        private readonly FacultyDbContext _context;

        public TraineeController(FacultyDbContext context)
        {
            _context = context;
        }

        // GET: Trainee (List of Trainees)
        public async Task<IActionResult> Index()
        {
            var trainees = await _context.Trainees.Include(t => t.Department).ToListAsync();
            return View(trainees);
        }

        // GET: Trainee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .Include(t => t.CourseResults)
                .ThenInclude(cr => cr.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainee == null) return NotFound();

            return View(trainee);
        }

        // GET: Trainee/Create
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name");
            return View();
        }

        // POST: Trainee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainee);
        }

        // GET: Trainee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null) return NotFound();

            ViewBag.Departments = new SelectList(_context.Departments.ToList(),"Id","Name");
            return View(trainee);
        }

        // POST: Trainee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trainee trainee)
        {
            if (id != trainee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(trainee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainee);
        }

        // GET: Trainee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainee == null) return NotFound();

            return View(trainee);
        }

        // POST: Trainee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainee = await _context.Trainees.FindAsync(id);
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null) return NotFound();

            var trainee = await _context.Trainees
                .Include(t => t.CourseResults)
                .ThenInclude(cr => cr.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainee == null) return NotFound();

            // Get courses the trainee is already enrolled in
            var enrolledCourseIds = trainee.CourseResults.Select(cr => cr.CourseId).ToList();

            // Get only courses the trainee is NOT enrolled in
            var availableCourses = _context.Courses.Where(c => !enrolledCourseIds.Contains(c.Id)).ToList();

            ViewBag.Courses = availableCourses;
            return View(trainee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int id, int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            var trainee = await _context.Trainees
                .Include(t => t.CourseResults)
                .ThenInclude(cr => cr.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainee == null || course == null) return NotFound();
            trainee.CourseResults.Add(new CourseResult { Course = course });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
