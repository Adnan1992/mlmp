using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{
    public class Assignmet
    {
        public int AssignmetID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public int BatchID { get; set; }
        public virtual Batch Batch { get; set; }
    }
}