using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        [Required(ErrorMessage =("Review is required"))]
        public string Text { get; set; }
        public int BatchID { get; set; }
        public virtual Batch Batch { get; set; }
        public string MadeBy { get; set; }
    }
}