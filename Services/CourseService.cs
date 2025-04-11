using FacultySystem.Data.Repositories;
using FacultySystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllWithInstructorAsync();
        }

        public async Task<Course> GetCourseDetailsAsync(int id)
        {
            return await _courseRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task CreateCourseAsync(Course course)
        {
            await _courseRepository.AddAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _courseRepository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course != null)
            {
                await _courseRepository.DeleteAsync(course);
            }
        }

        public async Task<IEnumerable<Trainee>> GetAvailableTraineesAsync(int courseId)
        {
            return await _courseRepository.GetAvailableTraineesForCourseAsync(courseId);
        }

        public async Task AddTraineeToCourseAsync(int courseId, int traineeId)
        {
            await _courseRepository.AddTraineeToCourseAsync(courseId, traineeId);
        }

        public async Task RemoveTraineeFromCourseAsync(int courseId, int traineeId)
        {
            await _courseRepository.RemoveTraineeFromCourseAsync(courseId, traineeId);
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            return await _courseRepository.GetAllInstructorsAsync();
        }
    }
}