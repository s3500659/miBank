using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class ATMViewModel
    {
        public Customer Customer { get; set; }
        public List<Account> Accounts { get; set; }
        public TransactionType TransactionType { get; set; }


    }
}
