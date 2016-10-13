using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        [Required, Display(Name = "Select Date"), DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }
        //public bool MarkAttendance { get; set; }
        public int EnrollmentID { get; set; }
        public virtual Enrollment Enrollment { get; set; }
        public int BatchID { get; set; }
        public string AttendanceStatus { get; set; }

    }
}
