using FacultySystem.Models;

namespace FacultySystem.Data.Repositories
{
    public interface ITraineeRepository
    {
        Task<IEnumerable<Trainee>> GetAllWithDepartmentAsync();
        Task<Trainee> GetByIdWithDetailsAsync(int id);
        Task<Trainee> GetByIdAsync(int id);
        Task AddAsync(Trainee trainee);
        Task UpdateAsync(Trainee trainee);
        Task DeleteAsync(Trainee trainee);
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<List<Course>> GetAvailableCoursesAsync(int traineeId);
        Task EnrollInCourseAsync(int traineeId, int courseId);
    }
}