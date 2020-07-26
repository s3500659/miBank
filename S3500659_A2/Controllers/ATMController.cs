using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NwbaExample.Utilities;
using S3500659_A2.Data;
using S3500659_A2.Filters;
using S3500659_A2.Models;
using S3500659_A2.ViewModel;

namespace S3500659_A2.Controllers
{
    [AuthorizeCustomer]
    public class ATMController : Controller
    {
        private readonly DBContext _context;


        // retrieve the session variable
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private Account SourceAccount;

        public ATMController(DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);

            return View(customer);
        }


        //public async Task<IActionResult> Deposit(int id) => View(await _context.Accounts.FindAsync(id));
        public async Task<IActionResult> Deposit(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(int id, decimal amount, string comment)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            account.Deposit(amount, comment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Withdraw(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount, string comment)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");

            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            if (account.TransactionCounter < account.MaxFreeTransaction)
            {
                if (account.Balance < amount)
                {
                    ModelState.AddModelError(nameof(amount), "You don't have enough money to make this withdraw");

                    if (!ModelState.IsValid)
                    {
                        ViewBag.Amount = amount;
                        return View(account);
                    }
                }
                account.Withdraw(amount, comment);
            }
            else
            {
                if (account.Balance < (amount + ServiceCharge.WithdrawFee))
                {
                    ModelState.AddModelError(nameof(amount), $"Your balance is less than requested withdraw amount + withdraw fee {ServiceCharge.WithdrawFee}");

                    if (!ModelState.IsValid)
                    {
                        ViewBag.Amount = amount;
                        return View(account);
                    }
                }
                else
                {
                    account.Withdraw(amount, comment, ServiceCharge.WithdrawFee);
                }
            }



            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Transfer(int id)
        {
            //var customer = await _context.Customers.FindAsync(CustomerID);
            //ViewBag.SourceAccount = id;

            //ViewData["AccountID"] = new SelectList(
            //    _context.Accounts
            //    .Where(t => t.CustomerID == HttpContext.Session.GetInt32("CustomerID"))
            //    .OrderBy(t => t.ModifyDate),
            //    "AccountID",
            //    "AccountID"
            //    );

            var customer = await _context.Customers.FindAsync(CustomerID);
            SourceAccount = await _context.Accounts.FindAsync(id);


            var model = new TransferViewModel
            {
                Customer = customer,
                Source = await _context.Accounts.FindAsync(id),
                Accounts = new SelectList(customer.Accounts, "AccountNumber", "AccountNumber")

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel viewModel)
        {
            Console.WriteLine(SourceAccount.ToString());

            return View(viewModel);
        }

        public async Task<IActionResult> NewATM()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);

            var viewModel = new NewATMViewModel
            {
                CustomerName = customer.CustomerName,
                AccountList = new SelectList(customer.Accounts, "AccountNumber", "AccountNumber")


            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> NewATM(NewATMViewModel viewModel)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            var selectList = new SelectList(customer.Accounts, "AccountNumber", "AccountNumber");

            var sourceAccount = await _context.Accounts.FindAsync(viewModel.SourceAccountNumber);
            var destinationAccount = await _context.Accounts.FindAsync(viewModel.DestAccountNumber);
            var amount = viewModel.Amount;
            var comment = viewModel.Comment;

            if (viewModel.Amount <= 0)
                ModelState.AddModelError(nameof(viewModel.Amount), "Amount must be positive.");
            if (viewModel.Amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(viewModel.Amount), "Amount cannot have more than 2 decimal places.");

            if (!ModelState.IsValid)
            {
                viewModel.AccountList = selectList;
                return View(viewModel);
            }


            if (viewModel.TransactionType == TransactionType.Deposit)
            {
                

                sourceAccount.Deposit(viewModel.Amount, viewModel.Comment);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else if (viewModel.TransactionType == TransactionType.Withdraw)
            {

                if (sourceAccount.TransactionCounter < sourceAccount.MaxFreeTransaction)
                {
                    if (sourceAccount.Balance < viewModel.Amount)
                    {
                        ModelState.AddModelError(nameof(viewModel.Amount), "You don't have enough money to make this withdraw");

                        if (!ModelState.IsValid)
                        {
                            viewModel.AccountList = selectList;
                            return View(viewModel);
                        }
                    }
                    sourceAccount.Withdraw(viewModel.Amount, viewModel.Comment);
                }
                else
                {
                    if (sourceAccount.Balance < (viewModel.Amount + ServiceCharge.WithdrawFee))
                    {
                        ModelState.AddModelError(nameof(viewModel.Amount), $"Your balance is less than requested withdraw amount + withdraw fee {ServiceCharge.WithdrawFee}");

                        if (!ModelState.IsValid)
                        {
                            viewModel.AccountList = selectList;
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        sourceAccount.Withdraw(viewModel.Amount, viewModel.Comment, ServiceCharge.WithdrawFee);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else if (viewModel.TransactionType == TransactionType.Transfer)
            {
                if (sourceAccount.TransactionCounter < sourceAccount.MaxFreeTransaction)
                {
                    if(sourceAccount.Balance < amount)
                    {
                        ModelState.AddModelError(nameof(viewModel.Amount), "You don't have enough funds to make this transfer");

                        if (!ModelState.IsValid)
                        {
                            viewModel.AccountList = selectList;
                            return View(viewModel);
                        }

                        sourceAccount.Transfer(destinationAccount, amount, comment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }

                if(sourceAccount.Balance < (amount + ServiceCharge.TransferFee))
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), $"Your balance is less than requested transfer amount + withdraw fee {ServiceCharge.TransferFee}");

                    if (!ModelState.IsValid)
                    {
                        viewModel.AccountList = selectList;
                        return View(viewModel);
                    }
                }

                sourceAccount.Transfer(destinationAccount, amount, comment, ServiceCharge.TransferFee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
           
            


            return View(viewModel);
        }


    }
}
