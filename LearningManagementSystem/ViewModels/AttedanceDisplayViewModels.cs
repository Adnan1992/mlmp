using LearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.ViewModels
{
    public class AttedanceDisplayViewModels
    {
        public IEnumerable<DateTime> ListDate { get; set; }
        public IEnumerable<string> ListStudent { get; set; }
        public IEnumerable<string> ListAtt { get; set; }
    }
}
