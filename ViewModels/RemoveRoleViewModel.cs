using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacultySystem.Models
{
    public class RemoveRoleViewModel
    {
        [Required(ErrorMessage = "Please select a user")]
        [Display(Name = "User Email")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public List<SelectListItem>? Users { get; set; }
        public List<SelectListItem>? Roles { get; set; }
    }
}