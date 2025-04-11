using FacultySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public interface ITraineeService
    {
        Task<IEnumerable<Trainee>> GetAllTraineesAsync();
        Task<Trainee> GetTraineeDetailsAsync(int id);
        Task<Trainee> GetTraineeByIdAsync(int id);
        Task CreateTraineeAsync(Trainee trainee);
        Task UpdateTraineeAsync(Trainee trainee);
        Task DeleteTraineeAsync(int id);
        Task<List<SelectListItem>> GetDepartmentDropdownAsync();
        Task<List<Course>> GetAvailableCoursesAsync(int traineeId);
        Task EnrollInCourseAsync(int traineeId, int courseId);
    }
}