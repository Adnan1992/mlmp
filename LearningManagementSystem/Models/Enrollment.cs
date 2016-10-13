using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int BatchID { get; set; }
        public int StudentID { get; set; }
        public string IsApproved { get; set; }
        public string UserId { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Student Student { get; set; }
        public string FullName { get { return Student.FirstName + " " + Student.LastName; } }
    }
}