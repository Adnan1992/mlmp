using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{
    public class StudentFee
    {
        public int StudentFeeID { get; set; }
        [RegularExpression("^([0-9]){1,12}$", ErrorMessage = "Numbers Only"), Required(ErrorMessage = "Amount is required")]
        public string Amount { get; set; }
        public int EnrollmentID { get; set; }
        public virtual Enrollment Enrollment { get; set; }
    }
}