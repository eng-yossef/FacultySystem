using FacultySystem.Models;

namespace FacultySystem.Data.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllWithInstructorAsync();
        Task<Course> GetByIdWithDetailsAsync(int id);
        Task<Course> GetByIdAsync(int id);
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(Course course);
        Task<IEnumerable<Trainee>> GetAvailableTraineesForCourseAsync(int courseId);
        Task AddTraineeToCourseAsync(int courseId, int traineeId);
        Task RemoveTraineeFromCourseAsync(int courseId, int traineeId);
        Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
    }
}