using FacultySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FacultySystem.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly FacultyDbContext _context;

        public CourseRepository(FacultyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllWithInstructorAsync()
        {
            return await _context.Courses.Include(c => c.Instructor).ToListAsync();
        }

        public async Task<Course> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.CourseResults)
                .ThenInclude(cr => cr.Trainee)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Trainee>> GetAvailableTraineesForCourseAsync(int courseId)
        {
            var enrolledTraineeIds = await _context.CourseResults
                .Where(cr => cr.CourseId == courseId)
                .Select(cr => cr.TraineeId)
                .ToListAsync();

            return await _context.Trainees
                .Where(t => !enrolledTraineeIds.Contains(t.Id))
                .ToListAsync();
        }

        public async Task AddTraineeToCourseAsync(int courseId, int traineeId)
        {
            var courseResult = new CourseResult { CourseId = courseId, TraineeId = traineeId };
            await _context.CourseResults.AddAsync(courseResult);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTraineeFromCourseAsync(int courseId, int traineeId)
        {
            var courseResult = await _context.CourseResults
                .FirstOrDefaultAsync(cr => cr.CourseId == courseId && cr.TraineeId == traineeId);

            if (courseResult != null)
            {
                _context.CourseResults.Remove(courseResult);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            return await _context.Instructors.ToListAsync();
        }
    }
}