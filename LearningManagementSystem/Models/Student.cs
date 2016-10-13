using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Gender { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Biography { get; set; }
    }
}