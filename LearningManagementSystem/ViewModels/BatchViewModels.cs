using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.ViewModels
{
    public class BatchViewModels
    {
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        [Display(Name = "Course Start Date"), DataType(DataType.Date)]
        public DateTime StartingDate { get; set; }
        [Display(Name = "Course Finishing Date"), DataType(DataType.Date)]
        public DateTime FinishingDate { get; set; }
        public int CourseID { get; set; }
    }
}