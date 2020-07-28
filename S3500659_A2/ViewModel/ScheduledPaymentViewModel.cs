using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class ScheduledPaymentViewModel
    {
        public Customer Customer { get; set; }
        public List<BillPay> BillPays { get; set; }
        public BillPay BillPay { get; set; }
    }
}
