using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.ViewModels
{
    public class ContactUsViewModels
    {
        [RegularExpression("^([a-zA-Z]){1,20}$", ErrorMessage = "Name Contains Only Letters with max length 20")]
        [Required(ErrorMessage = "First Name is required."), MaxLength(20), Display(Name = "First Name")]
        public string Name { get; set; }


        [RegularExpression("^([0-9]){9,12}$", ErrorMessage = "Numbers Only with length 9-12")]
        [Required, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }



        [Required, Display(Name = "Email")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,3})$", ErrorMessage = "Email is not a valid e-mail address.")]
        public string Email { get; set; }


        [Required, Display(Name = "Your Message")]
        public string Message { get; set; }


        [RegularExpression("^([a-zA-Z]){2,30}$", ErrorMessage = "Subject Contains Only Letters with max length 30")]
        [Required, MaxLength(30), Display(Name = "Subject")]
        public string Subject { get; set; }
    }
}