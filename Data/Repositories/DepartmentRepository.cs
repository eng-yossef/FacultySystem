using FacultySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FacultySystem.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly FacultyDbContext _context;

        public DepartmentRepository(FacultyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department> GetByIdWithInstructorsAndTraineesAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Instructors)
                .Include(d => d.Trainees)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Department entity)
        {
            await _context.Departments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department entity)
        {
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department entity)
        {
            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Instructor>> GetAvailableInstructorsAsync()
        {
            return await _context.Instructors
                .Where(i => i.DepartmentId == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trainee>> GetAvailableTraineesAsync()
        {
            return await _context.Trainees
                .Where(t => t.DepartmentId == null)
                .ToListAsync();
        }

        public async Task AddInstructorToDepartmentAsync(int departmentId, int instructorId)
        {
            var instructor = await _context.Instructors.FindAsync(instructorId);
            if (instructor != null)
            {
                instructor.DepartmentId = departmentId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTraineeToDepartmentAsync(int departmentId, int traineeId)
        {
            var trainee = await _context.Trainees.FindAsync(traineeId);
            if (trainee != null)
            {
                trainee.DepartmentId = departmentId;
                await _context.SaveChangesAsync();
            }
        }
    }
}