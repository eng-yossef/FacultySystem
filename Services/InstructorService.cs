using FacultySystem.Data.Repositories;
using FacultySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacultySystem.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InstructorService(
            IInstructorRepository instructorRepository,
            IImageRepository imageRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _instructorRepository = instructorRepository;
            _imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            return await _instructorRepository.GetAllWithDepartmentAsync();
        }

        public async Task<Instructor> GetInstructorDetailsAsync(int id)
        {
            return await _instructorRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Instructor> GetInstructorByIdAsync(int id)
        {
            return await _instructorRepository.GetByIdAsync(id);
        }

        public async Task CreateInstructorAsync(Instructor instructor, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                instructor.ImageUrl = await _imageRepository.SaveImageAsync(imageFile, _webHostEnvironment.WebRootPath);
            }

            await _instructorRepository.AddAsync(instructor);
        }

        public async Task UpdateInstructorAsync(int id, Instructor instructor, IFormFile imageFile)
        {
            var existingInstructor = await _instructorRepository.GetByIdAsync(id);
            if (existingInstructor == null) return;

            if (imageFile != null && imageFile.Length > 0)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingInstructor.ImageUrl))
                {
                    await _imageRepository.DeleteImageAsync(existingInstructor.ImageUrl, _webHostEnvironment.WebRootPath);
                }

                // Save new image
                instructor.ImageUrl = await _imageRepository.SaveImageAsync(imageFile, _webHostEnvironment.WebRootPath);
            }
            else
            {
                // Keep the old image
                instructor.ImageUrl = existingInstructor.ImageUrl;
            }

            await _instructorRepository.UpdateAsync(instructor);
        }

        public async Task DeleteInstructorAsync(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null) return;

            // Delete image if it exists
            if (!string.IsNullOrEmpty(instructor.ImageUrl))
            {
                await _imageRepository.DeleteImageAsync(instructor.ImageUrl, _webHostEnvironment.WebRootPath);
            }

            await _instructorRepository.DeleteAsync(instructor);
        }

        public async Task<List<SelectListItem>> GetDepartmentDropdownAsync()
        {
            var departments = await _instructorRepository.GetAllDepartmentsAsync();
            return departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
        }

        public async Task<bool> ValidateNameAsync(string name)
        {
            // Implement your custom validation logic here
            // Currently returns true as per original implementation
            return await Task.FromResult(true);
        }
    }
}