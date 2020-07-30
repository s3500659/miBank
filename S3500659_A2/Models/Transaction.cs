using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.Models
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3,
        ServiceCharge = 4,
        BillPay = 5
    }


    public class Transaction
    {
        [Display(Name = "ID")]
        public int TransactionID { get; set; }

        [Required]
        [Display(Name = "Type")]
        public TransactionType TransactionType { get; set; }

        [ForeignKey("Account")]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        
        public virtual Account Account { get; set; }

        [ForeignKey("DestinationAccount")]
        [Display(Name = "Destination Account")]
        public int? DestinationAccountNumber { get; set; }
        public virtual Account DestinationAccount { get; set; }


        [Required, DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [Required]
        [StringLength(8)]
        [Display(Name = "Transaction Date")]
        public DateTime ModifyDate { get; set; }


    }
}
