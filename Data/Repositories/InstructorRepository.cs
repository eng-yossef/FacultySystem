using Microsoft.AspNetCore.Http;
using FacultySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FacultySystem.Data.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly FacultyDbContext _context;

        public InstructorRepository(FacultyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Instructor>> GetAllWithDepartmentAsync()
        {
            return await _context.Instructors.Include(i => i.Department).ToListAsync();
        }

        public async Task<Instructor> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Instructor> GetByIdAsync(int id)
        {
            return await _context.Instructors.FindAsync(id);
        }

        public async Task AddAsync(Instructor instructor)
        {
            await _context.Instructors.AddAsync(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Instructor instructor)
        {
            _context.Instructors.Update(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Instructor instructor)
        {
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }
    }
}


namespace FacultySystem.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        public async Task<string> SaveImageAsync(IFormFile imageFile, string webRootPath)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(webRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/{uniqueFileName}";
        }

        public async Task DeleteImageAsync(string imageUrl, string webRootPath)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            string imagePath = Path.Combine(webRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                await Task.Run(() => System.IO.File.Delete(imagePath));
            }
        }
    }
}