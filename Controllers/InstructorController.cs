using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultySystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacultySystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly FacultyDbContext _context;

        public InstructorController(FacultyDbContext context)
        {
            _context = context;
        }

        // GET: Instructor (List of Instructors)
        public async Task<IActionResult> Index()
        {
            var instructors = await _context.Instructors.Include(i => i.Department).ToListAsync();
            return View(instructors);
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return NotFound();

            ViewBag.Departments = _context.Departments.ToList();
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor)
        {
            if (id != instructor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
