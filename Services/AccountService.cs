using FacultySystem.Data.Repositories;
using FacultySystem.Models;
using FacultySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacultySystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly FacultyDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            FacultyDbContext context,
            SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<List<SelectListItem>> GetDepartmentsAsync()
        {
            return await _accountRepository.GetDepartmentsAsync();
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model, IFormFile imageFile, string webRootPath)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (!result.Succeeded) return result;

            var roleResult = await _userRepository.AddToRoleAsync(user, model.Role);
            if (!roleResult.Succeeded) return roleResult;

            if (model.Role == "Instructor")
            {
                var instructor = new Instructor
                {
                    Name = model.FullName,
                    Specialization = model.Specialization,
                    DepartmentId = model.DepartmentId,
                    UserId = user.Id,
                    ImageUrl = await _accountRepository.SaveImageAsync(imageFile, webRootPath)
                };
                _context.Instructors.Add(instructor);
            }
            else if (model.Role == "Trainee")
            {
                var trainee = new Trainee
                {
                    Name = model.FullName,
                    Age = model.Age,
                    DepartmentId = model.DepartmentId,
                    UserId = user.Id
                };
                _context.Trainees.Add(trainee);
            }

            await _context.SaveChangesAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);

            return IdentityResult.Success;
        }

        public async Task<SignInResult> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userRepository.FindByEmailAsync(model.Email);
            if (user == null) return SignInResult.Failed;

            var passwordValid = await _userRepository.CheckPasswordAsync(user, model.Password);
            if (!passwordValid) return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}