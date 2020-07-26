using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using S3500659_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new DBContext(serviceProvider.GetRequiredService<DbContextOptions<DBContext>>());

            if (context.Customers.Any())
                return;

            context.Customers.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    CustomerName = "Matthew Bolger",
                    TFN = "10000000000",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "3000",
                    Phone = "0408378888"
                },
                new Customer
                {
                    CustomerID = 2200,
                    CustomerName = "Rodney Cocker",
                    TFN = "10000000001",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "3000",
                    Phone = "0408378881"
                });


            //const string format = "dd/MM/yyyy hh:mm:ss tt";
            context.Logins.AddRange(
                new Login
                {
                    CustomerID = 2100,
                    LoginID = "12345678",
                    Password = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                    ModifyDate = DateTime.UtcNow


                },
                new Login
                {
                    CustomerID = 2200,
                    LoginID = "38074569",
                    Password = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                    ModifyDate = DateTime.UtcNow
                });

            context.Accounts.AddRange(
                new Account
                {
                    AccountNumber = 4100,
                    AccountType = AccountType.Saving,
                    CustomerID = 2100,
                    Balance = 100,
                    ModifyDate = DateTime.UtcNow,
                    MaxFreeTransaction = 4

                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 500,
                    ModifyDate = DateTime.UtcNow,
                    MaxFreeTransaction = 4
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = AccountType.Saving,
                    CustomerID = 2200,
                    Balance = 500.95m,
                    ModifyDate = DateTime.UtcNow,
                    MaxFreeTransaction = 4
                });

            context.Transactions.AddRange(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null

                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 500,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 0.95M,
                    Comment = null,
                    ModifyDate = DateTime.UtcNow,
                    DestinationAccountNumber = null
                });


            context.SaveChanges();
        }
    }
}
