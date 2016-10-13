using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.ViewModels
{
    public class InstructorViewModels
    {
        public int InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Gender { get; set; }
        public string UserId { get; set; }
        [RegularExpression("^([0-9]){11}$", ErrorMessage = "Numbers Only with length 11")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Biography { get; set; }
        public string Certificates { get; set; }
        public string SocialLinks { get; set; }
    }
}