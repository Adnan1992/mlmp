using LearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LearningManagementSystem.ViewModels
{
    public class AttendanceViewModels
    {
        //public AttendanceViewModels()
        //{
        //    Course = new Course();
        //    Batch = new Batch();
        //}
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        public List<DateTime> AttDate { get; set; }
        public List<string> StName { get; set; }
        public List<string> AttStatus { get; set; }
    }
}
