using FacultySystem.Models;

namespace FacultySystem.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<Department> GetDepartmentDetailsAsync(int id);
        Task CreateDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int id);
        Task<IEnumerable<Instructor>> GetAvailableInstructorsAsync();
        Task<IEnumerable<Trainee>> GetAvailableTraineesAsync();
        Task AddInstructorToDepartmentAsync(int departmentId, int instructorId);
        Task AddTraineeToDepartmentAsync(int departmentId, int traineeId);
    }
}