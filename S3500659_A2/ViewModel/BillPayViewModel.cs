using Microsoft.AspNetCore.Mvc.Rendering;
using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class BillPayViewModel
    {
        public Customer Customer { get; set; }
        public SelectList AccountList { get; set; }
        public SelectList PayeeList { get; set; }

        [Required]
        public int AccountNumber { get; set; }
        [Required]
        public int PayeeId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime SecheduledDate { get; set; }
        [Required]
        public string Period { get; set; }
    }
}
