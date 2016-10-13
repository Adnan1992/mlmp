﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Gender { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Biography { get; set; }
        public string Certificates { get; set; }
        public string SocialLinks { get; set; }
    }
}