using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.Models
{
    public enum Period
    {
        Minute,
        Quarterly,
        Annually,
        OnceOff
    }

    public class BillPay
    {
        [Required]
        public int BillPayID { get; set; }

        [Required]
        public virtual Account Account { get; set; }

        [Required]
        public virtual Payee Payee { get; set; }

        [Required, StringLength(8), DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required, StringLength(8)]
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate { get; set; }

        [Required, StringLength(8)]
        public DateTime ModifyDate { get; set; }

        [Required]
        public Period Period { get; set; }



    }
}
