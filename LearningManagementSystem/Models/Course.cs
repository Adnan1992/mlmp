using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LearningManagementSystem.Models
{
    //[DataContract(IsReference = true)]
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CourseID { get; set; }
        [Required, DataType(DataType.Text)]
        public string Title { get; set; }



        [Required, AllowHtml]
        [StringLength(5000, ErrorMessage = "Description Max Length is 5000")]
        public string Description { get; set; }

        public string ImageName { get; set; }
        public string Path { get; set; }
    }
}