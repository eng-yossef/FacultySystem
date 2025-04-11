using FacultySystem.Models;

namespace FacultySystem.Data.Repositories
{
    public interface IInstructorRepository
    {
        Task<IEnumerable<Instructor>> GetAllWithDepartmentAsync();
        Task<Instructor> GetByIdWithDetailsAsync(int id);
        Task<Instructor> GetByIdAsync(int id);
        Task AddAsync(Instructor instructor);
        Task UpdateAsync(Instructor instructor);
        Task DeleteAsync(Instructor instructor);
        Task<List<Department>> GetAllDepartmentsAsync();
    }
}

namespace FacultySystem.Data.Repositories
{
    public interface IImageRepository
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string webRootPath);
        Task DeleteImageAsync(string imageUrl, string webRootPath);
    }
}