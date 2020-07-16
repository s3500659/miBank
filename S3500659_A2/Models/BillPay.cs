using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.Models
{
    public class BillPay
    {
        [Required]
        public int BillPayID { get; set; }

        //[Required, StringLength(4)]
        //public int AccountNumber { get; set; }

        [Required]
        public virtual Account Account { get; set; }

        //[Required, StringLength(4)]
        //public int PayeeID { get; set; }
        [Required]
        public virtual Payee Payee { get; set; }

        [Required, StringLength(8), DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required, StringLength(8)]
        public DateTime ScheduleDate { get; set; }

        [Required, StringLength(1)]
        public string Period { get; set; }

        [Required, StringLength(8)]
        public DateTime ModifyDate { get; set; }



    }
}
