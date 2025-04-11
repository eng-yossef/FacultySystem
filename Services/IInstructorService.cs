using FacultySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public interface IInstructorService
    {
        Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
        Task<Instructor> GetInstructorDetailsAsync(int id);
        Task<Instructor> GetInstructorByIdAsync(int id);
        Task CreateInstructorAsync(Instructor instructor, IFormFile imageFile);
        Task UpdateInstructorAsync(int id, Instructor instructor, IFormFile imageFile);
        Task DeleteInstructorAsync(int id);
        Task<List<SelectListItem>> GetDepartmentDropdownAsync();
        Task<bool> ValidateNameAsync(string name);
    }
}