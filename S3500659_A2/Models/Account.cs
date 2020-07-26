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

    public static class ServiceCharge
    {
        public static decimal WithdrawFee { get; set; } = 0.10M;
        public static decimal TransferFee { get; set; } = 0.20M;
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
        public int TransactionCounter { get; set; }
        public int MaxFreeTransaction { get; set; }

        public Account()
        {
            TransactionCounter = 0;
            MaxFreeTransaction = 4;
        }

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

        public void Withdraw(decimal amount, string comment)
        {
            Balance -= amount;
            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow,
                    Comment = comment

                });
            TransactionCounter++;
        }

        public void Withdraw(decimal amount, string comment, decimal fee)
        {
            Balance -= (amount + fee);
            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow,
                    Comment = comment

                });

            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.ServiceCharge,
                    Amount = fee,
                    ModifyDate = DateTime.UtcNow,
                    Comment = "Withdraw Fee"

                });

            TransactionCounter++;
        }

        public void Transfer(Account destination, decimal amount, string comment, decimal fee = 0)
        {
            Balance -= (amount + fee);
            TransactionCounter++;

            destination.Balance += amount;

            Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    Amount = -amount,
                    DestinationAccountNumber = destination.AccountNumber,
                    ModifyDate = DateTime.UtcNow,
                    Comment = comment
                });

            destination.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    Amount = amount,
                    ModifyDate = DateTime.UtcNow,
                    Comment = comment
                });

        }
    }
}