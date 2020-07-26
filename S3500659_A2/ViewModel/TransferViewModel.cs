using Microsoft.AspNetCore.Mvc.Rendering;
using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class TransferViewModel
    {
        public Customer Customer { get; set; }
        public Account Source { get; set; }
        public int Destination { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public SelectList Accounts { get; set; }



    }
}
