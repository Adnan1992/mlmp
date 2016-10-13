using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.ViewModels
{
    public class EnrollmentViewModels
    {
        public int BatchId { get; set; }
        public string Path  { get; set; }
        public string IsApproved { get; set; }
        //[Required, DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "Course Start Date"), DataType(DataType.Date)]
        public DateTime StartingDate { get; set; }
        [Display(Name = "Course Finishing Date"), DataType(DataType.Date)]
        public DateTime FinishingDate { get; set; }
        public bool Status { get; set; }
        //[Required, DataType(DataType.MultilineText)]
        //[StringLength(255, ErrorMessage = "Description Max Length is 255")]
        public string Description { get; set; }
        public string UserId { get; set; }
        public string  InstName { get; set; }
        public bool CheckStatus { get; set; }
    }
}
