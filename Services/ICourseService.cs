using FacultySystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseDetailsAsync(int id);
        Task<Course> GetCourseByIdAsync(int id);
        Task CreateCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        Task<IEnumerable<Trainee>> GetAvailableTraineesAsync(int courseId);
        Task AddTraineeToCourseAsync(int courseId, int traineeId);
        Task RemoveTraineeFromCourseAsync(int courseId, int traineeId);
        Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
    }
}