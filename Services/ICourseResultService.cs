using FacultySystem.Models;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public interface ICourseResultService
    {
        Task<CourseResult> GetCourseResultAsync(int courseId, int traineeId);
        Task RemoveStudentFromCourseAsync(int courseId, int traineeId);
        Task UpdateGradeAsync(int courseId, int traineeId, float newGrade);
    }
}