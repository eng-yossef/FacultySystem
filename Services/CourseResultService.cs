using FacultySystem.Data.Repositories;
using FacultySystem.Models;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public class CourseResultService : ICourseResultService
    {
        private readonly ICourseResultRepository _courseResultRepository;

        public CourseResultService(ICourseResultRepository courseResultRepository)
        {
            _courseResultRepository = courseResultRepository;
        }

        public async Task<CourseResult> GetCourseResultAsync(int courseId, int traineeId)
        {
            return await _courseResultRepository.GetByIdAsync(courseId, traineeId);
        }

        public async Task RemoveStudentFromCourseAsync(int courseId, int traineeId)
        {
            var courseResult = await _courseResultRepository.GetByIdAsync(courseId, traineeId);
            if (courseResult != null)
            {
                await _courseResultRepository.DeleteAsync(courseResult);
            }
        }

        public async Task UpdateGradeAsync(int courseId, int traineeId, float newGrade)
        {
            await _courseResultRepository.UpdateGradeAsync(courseId, traineeId, newGrade);
        }
    }
}