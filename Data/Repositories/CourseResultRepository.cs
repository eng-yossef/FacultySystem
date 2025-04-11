using FacultySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FacultySystem.Data.Repositories
{
    public class CourseResultRepository : ICourseResultRepository
    {
        private readonly FacultyDbContext _context;

        public CourseResultRepository(FacultyDbContext context)
        {
            _context = context;
        }

        public async Task<CourseResult> GetByIdAsync(int courseId, int traineeId)
        {
            return await _context.CourseResults
                .Include(cr => cr.Course)
                .Include(cr => cr.Trainee)
                .FirstOrDefaultAsync(cr => cr.CourseId == courseId && cr.TraineeId == traineeId);
        }

        public async Task DeleteAsync(CourseResult courseResult)
        {
            _context.CourseResults.Remove(courseResult);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGradeAsync(int courseId, int traineeId, float newGrade)
        {
            var courseResult = await _context.CourseResults
                .FirstOrDefaultAsync(cr => cr.CourseId == courseId && cr.TraineeId == traineeId);

            if (courseResult != null)
            {
                courseResult.Grade = newGrade;
                await _context.SaveChangesAsync();
            }
        }
    }
}