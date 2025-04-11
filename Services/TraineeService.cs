using FacultySystem.Data.Repositories;
using FacultySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ITraineeRepository _traineeRepository;

        public TraineeService(ITraineeRepository traineeRepository)
        {
            _traineeRepository = traineeRepository;
        }

        public async Task<IEnumerable<Trainee>> GetAllTraineesAsync()
        {
            return await _traineeRepository.GetAllWithDepartmentAsync();
        }

        public async Task<Trainee> GetTraineeDetailsAsync(int id)
        {
            return await _traineeRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Trainee> GetTraineeByIdAsync(int id)
        {
            return await _traineeRepository.GetByIdAsync(id);
        }

        public async Task CreateTraineeAsync(Trainee trainee)
        {
            await _traineeRepository.AddAsync(trainee);
        }

        public async Task UpdateTraineeAsync(Trainee trainee)
        {
            await _traineeRepository.UpdateAsync(trainee);
        }

        public async Task DeleteTraineeAsync(int id)
        {
            var trainee = await _traineeRepository.GetByIdAsync(id);
            if (trainee != null)
            {
                await _traineeRepository.DeleteAsync(trainee);
            }
        }

        public async Task<List<SelectListItem>> GetDepartmentDropdownAsync()
        {
            var departments = await _traineeRepository.GetAllDepartmentsAsync();
            return departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
        }

        public async Task<List<Course>> GetAvailableCoursesAsync(int traineeId)
        {
            return await _traineeRepository.GetAvailableCoursesAsync(traineeId);
        }

        public async Task EnrollInCourseAsync(int traineeId, int courseId)
        {
            await _traineeRepository.EnrollInCourseAsync(traineeId, courseId);
        }
    }
}