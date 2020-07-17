using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace S3500659_A2.Models
{
    public enum AccountType
    {
        Saving = 1,
        Checking = 2
    }

    public class Account
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountNumber { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        [Required]
        public DateTime ModifyDate { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

        [Required, DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Balance { get; set; }

        public void Deposit(decimal amount, string comment)
        {
            Balance += amount;
            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow,
                    Comment = comment
                    
                });
        }
    }
}