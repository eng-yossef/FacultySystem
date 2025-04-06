using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultySystem.Models;

namespace FacultySystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly FacultyDbContext _context;

        public DepartmentController(FacultyDbContext context)
        {
            _context = context;
        }

        // GET: Departments (List all Departments)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments
                .Include(d => d.Instructors)
                .Include(d => d.Trainees)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null) return NotFound();

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        //Add Instructor to Department 

        public IActionResult AddInstructor(int departmentId)
        {
            var department = _context.Departments.Find(departmentId);
            if (department == null) return NotFound();

            var instructors = _context.Instructors
                .Where(i => i.DepartmentId == null)
                .ToList();

            ViewBag.DepartmentName = department.Name;
            ViewBag.DepartmentId = departmentId;  

            return View(instructors);
        }

        [HttpPost]
        public async Task<IActionResult> AddInstructor(int departmentId, int instructorId)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            var instructor = await _context.Instructors.FindAsync(instructorId);
            if (department == null || instructor == null) return NotFound();
            instructor.DepartmentId = departmentId;
            _context.Update(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = departmentId });
        }


        //Add Trainee to Department
        public IActionResult AddTrainee(int departmentId)
        {
            var department = _context.Departments.Find(departmentId);
            if (department == null) return NotFound();
            var trainees = _context.Trainees
                .Where(t => t.DepartmentId == null)
                .ToList();
            ViewBag.DepartmentName = department.Name;
            ViewBag.DepartmentId = departmentId;
            return View(trainees);
        }
        [HttpPost]
        public async Task<IActionResult> AddTrainee(int departmentId, int traineeId)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            var trainee = await _context.Trainees.FindAsync(traineeId);
            if (department == null || trainee == null) return NotFound();
            trainee.DepartmentId = departmentId;
            _context.Update(trainee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = departmentId });
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Departments.Any(d => d.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
