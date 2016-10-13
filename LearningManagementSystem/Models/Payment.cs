using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{
    public class Payment
    {
        [ForeignKey("Batch")]
        public int PaymentID { get; set; }
        [RegularExpression("^([0-9]){1,12}$", ErrorMessage = "Numbers Only"), Required(ErrorMessage = "Amount is required")]
        public string Amount { get; set; }
        public string PaymentForBatch { get; set; }
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        public virtual Batch Batch { get; set; }
    }
}