using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.ViewModels
{
    public class EmployeeViewModel
    {
        [Required(ErrorMessage = " Firstname is Required"), RegularExpression("^([a-zA-Z]){2,15}$", ErrorMessage = ("Letters Only with max 15 characters"))]
        [Display(Name = "Fitst Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " Lasttname is Required"), RegularExpression("^([a-zA-Z]){2,15}$", ErrorMessage = ("Letters Only with max 15 characters"))]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,3})$", ErrorMessage = "Email is not a valid e-mail address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string USERID { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirmpassword { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Instructor Id")]
        public string InstructorId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        [RegularExpression("^([0-9]){11}$", ErrorMessage = "Numbers Only with length 11")]
        [Display(Name = "Contact Number"), Required(ErrorMessage = "Contact # is required")]
        public string ContactNumber { get; set; }
    }
    public class EmployeeUpdateViewModel
    {
        public string Role { get; set; }
        [Key]
        public string EmpId { get; set; }
        [Display(Name = "Fitst Name")]
        public string FirstName { get; set; }

        [MaxLength]
        public string PhotoFile { get; set; }
        [Display(Name = "Last Name")]

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirmpassword { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Instructor Id")]
        public string InstructorId { get; set; }

        [Display(Name = "Contact Number"), DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }
    }


    public class RegisterViewModel
    {
        [Required(ErrorMessage = " Firstname is Required"), RegularExpression("^([a-zA-Z]){2,15}$", ErrorMessage = ("Letters Only with max 15 characters"))]
        [Display(Name = "Fitst Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " Lastname is Required"), RegularExpression("^([a-zA-Z]){2,15}$", ErrorMessage = ("Letters Only with max 15 characters"))]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,3})$", ErrorMessage = "Email is not a valid e-mail address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Gender"), Required]
        public string Gender { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirmpassword { get; set; }
        [RegularExpression("^([0-9]){9,12}$", ErrorMessage = "Numbers Only with length 9-12")]
        [Display(Name = "Contact Number"), Required(ErrorMessage = "Contact # is required")]
        public string ContactNumber { get; set; }
    }


    public class RegisterAdmin
    {
        [Required(ErrorMessage = " Firstname is Required"), RegularExpression("^([a-zA-Z]){2,15}$", ErrorMessage = ("Letters Only with max 15 characters"))]
        [Display(Name = "Fitst Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " Lasttname is Required"), RegularExpression("^([a-zA-Z]){2,15}$", ErrorMessage = ("Letters Only with max 15 characters"))]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,3})$", ErrorMessage = "Email is not a valid e-mail address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string USERID { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirmpassword { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        //[RegularExpression("^([0-9]){11}$", ErrorMessage = "Numbers Only with length 11")]
        //[Display(Name = "Contact Number"), Required(ErrorMessage = "Contact # is required")]
        //public string ContactNumber { get; set; }
    }
}