using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class ATMViewModel
    {

        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }

        public List<Account> Accounts { get; set; }

        public string CustomerName { get; set; }

        [Display(Name = "Source Account")]
        public int SourceAccountNumber { get; set; }

        [Display(Name = "Destination Account")]
        public int? DestAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }


        
    }
}
