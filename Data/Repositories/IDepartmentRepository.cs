using FacultySystem.Models;

namespace FacultySystem.Data.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department> GetByIdWithInstructorsAndTraineesAsync(int id);
        Task<IEnumerable<Instructor>> GetAvailableInstructorsAsync();
        Task<IEnumerable<Trainee>> GetAvailableTraineesAsync();
        Task AddInstructorToDepartmentAsync(int departmentId, int instructorId);
        Task AddTraineeToDepartmentAsync(int departmentId, int traineeId);
    }
}