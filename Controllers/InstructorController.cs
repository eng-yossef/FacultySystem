using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultySystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FacultySystem.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        private readonly FacultyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public InstructorController(FacultyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: Instructor (List of Instructors)
        [AllowAnonymous]
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

        // GET: Instructors/Create
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", instructor.DepartmentId);
                return View(instructor);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                instructor.ImageUrl = "/images/" + uniqueFileName;
            }

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.Department) // Include department details
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            // Pass departments list to ViewBag for dropdown selection
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", instructor.DepartmentId);

            return View(instructor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor, IFormFile? imageFile)
        {
            if (id != instructor.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", instructor.DepartmentId);
                return View(instructor);
            }

            try
            {
                // Retrieve the existing instructor from the database
                var existingInstructor = await _context.Instructors.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
                if (existingInstructor == null) return NotFound();

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "images");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(existingInstructor.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", existingInstructor.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    instructor.ImageUrl = "/images/" + uniqueFileName; // Assign new image URL
                }
                else
                {
                    // Keep the old image if no new image is uploaded
                    instructor.ImageUrl = existingInstructor.ImageUrl;
                }

                _context.Update(instructor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Instructors.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }



        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null) return NotFound();

            return View(instructor);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return NotFound();

            // Delete the instructor's image if it exists
            if (!string.IsNullOrEmpty(instructor.ImageUrl))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", instructor.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Remove instructor from database
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
