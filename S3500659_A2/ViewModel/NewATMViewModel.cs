using Microsoft.AspNetCore.Mvc.Rendering;
using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class NewATMViewModel
    {
        public TransactionType TransactionType { get; set; }
        public string CustomerName { get;set; }
        public SelectList AccountList { get; set; }
        public int SourceAccountNumber { get; set; }
        public int DestAccountNumber { get; set; }

        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
