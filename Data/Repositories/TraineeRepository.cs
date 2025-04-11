using FacultySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FacultySystem.Data.Repositories
{
    public class TraineeRepository : ITraineeRepository
    {
        private readonly FacultyDbContext _context;

        public TraineeRepository(FacultyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trainee>> GetAllWithDepartmentAsync()
        {
            return await _context.Trainees.Include(t => t.Department).ToListAsync();
        }

        public async Task<Trainee> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Trainees
                .Include(t => t.Department)
                .Include(t => t.CourseResults)
                .ThenInclude(cr => cr.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Trainee> GetByIdAsync(int id)
        {
            return await _context.Trainees.FindAsync(id);
        }

        public async Task AddAsync(Trainee trainee)
        {
            await _context.Trainees.AddAsync(trainee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trainee trainee)
        {
            _context.Trainees.Update(trainee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Trainee trainee)
        {
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<List<Course>> GetAvailableCoursesAsync(int traineeId)
        {
            var enrolledCourseIds = await _context.CourseResults
                .Where(cr => cr.TraineeId == traineeId)
                .Select(cr => cr.CourseId)
                .ToListAsync();

            return await _context.Courses
                .Where(c => !enrolledCourseIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task EnrollInCourseAsync(int traineeId, int courseId)
        {
            var courseResult = new CourseResult { CourseId = courseId, TraineeId = traineeId };
            await _context.CourseResults.AddAsync(courseResult);
            await _context.SaveChangesAsync();
        }
    }
}