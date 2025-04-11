using FacultySystem.Models;

namespace FacultySystem.Data.Repositories
{
    public interface ICourseResultRepository
    {
        Task<CourseResult> GetByIdAsync(int courseId, int traineeId);
        Task DeleteAsync(CourseResult courseResult);
        Task UpdateGradeAsync(int courseId, int traineeId, float newGrade);
    }
}