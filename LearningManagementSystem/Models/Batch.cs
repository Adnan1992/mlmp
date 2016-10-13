using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Models
{
    public  class Batch
    {
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        [Display(Name = "Course Start Date"), DataType(DataType.Date)]
        public DateTime StartingDate { get; set; }
        [Display(Name = "Course Finishing Date"), DataType(DataType.Date)]
        public DateTime FinishingDate { get; set; }
        [Display(Name = "Course Title")]
        public string Title { get; set; }
        [Display(Name = "Course Description")]
        public string Description { get; set; }
        public string InstructorNamme { get; set; }
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        public virtual Payment Payment { get; set; }
    }
}
