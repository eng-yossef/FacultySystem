using FacultySystem.Data.Repositories;
using FacultySystem.Models;

namespace FacultySystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task<Department> GetDepartmentDetailsAsync(int id)
        {
            return await _departmentRepository.GetByIdWithInstructorsAndTraineesAsync(id);
        }

        public async Task CreateDepartmentAsync(Department department)
        {
            await _departmentRepository.AddAsync(department);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department != null)
            {
                await _departmentRepository.DeleteAsync(department);
            }
        }

        public async Task<IEnumerable<Instructor>> GetAvailableInstructorsAsync()
        {
            return await _departmentRepository.GetAvailableInstructorsAsync();
        }

        public async Task<IEnumerable<Trainee>> GetAvailableTraineesAsync()
        {
            return await _departmentRepository.GetAvailableTraineesAsync();
        }

        public async Task AddInstructorToDepartmentAsync(int departmentId, int instructorId)
        {
            await _departmentRepository.AddInstructorToDepartmentAsync(departmentId, instructorId);
        }

        public async Task AddTraineeToDepartmentAsync(int departmentId, int traineeId)
        {
            await _departmentRepository.AddTraineeToDepartmentAsync(departmentId, traineeId);
        }
    }
}