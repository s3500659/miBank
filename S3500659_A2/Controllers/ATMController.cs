using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public ATMController(DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);

            return View(customer);
        }


        public async Task<IActionResult> ATM()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);

            var viewModel = new ATMViewModel
            {
                CustomerName = customer.CustomerName,
                Accounts = await _context.Accounts.Where(x => x.CustomerID == customer.CustomerID).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ATM(ATMViewModel viewModel)
        {

            var customer = await _context.Customers.FindAsync(CustomerID);
            var sourceAccount = await _context.Accounts.FindAsync(viewModel.SourceAccountNumber);
            var destinationAccount = await _context.Accounts.FindAsync(viewModel.DestAccountNumber);
            var amount = viewModel.Amount;
            var comment = viewModel.Comment;

            viewModel.CustomerName = customer.CustomerName;
            viewModel.Accounts = await _context.Accounts.Where(x => x.CustomerID == customer.CustomerID).ToListAsync();

            if (viewModel.Amount <= 0)
                ModelState.AddModelError(nameof(viewModel.Amount), "Amount must be positive.");
            if (viewModel.Amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(viewModel.Amount), "Amount cannot have more than 2 decimal places.");

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Deposit
            if (viewModel.TransactionType == TransactionType.Deposit)
            {
                sourceAccount.Deposit(viewModel.Amount, viewModel.Comment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            // Withdraw
            else if (viewModel.TransactionType == TransactionType.Withdraw)
            {
                if (sourceAccount.TransactionCounter < sourceAccount.MaxFreeTransaction)
                {
                    if (sourceAccount.Balance < viewModel.Amount)
                    {
                        ModelState.AddModelError(nameof(viewModel.Amount), "You don't have enough money to make this withdraw");

                        return View(viewModel);
                    }
                    sourceAccount.Withdraw(viewModel.Amount, viewModel.Comment);
                }
                else
                {
                    if (sourceAccount.Balance < (viewModel.Amount + ServiceCharge.WithdrawFee))
                    {
                        ModelState.AddModelError(nameof(viewModel.Amount), $"Your balance is less than requested withdraw amount + withdraw fee ({ServiceCharge.WithdrawFee})");

                        return View(viewModel);
                    }
                    else
                    {
                        sourceAccount.Withdraw(viewModel.Amount, viewModel.Comment, ServiceCharge.WithdrawFee);
                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            // Transfer
            else if (viewModel.TransactionType == TransactionType.Transfer)
            {
                if (sourceAccount.TransactionCounter < sourceAccount.MaxFreeTransaction)
                {
                    if (sourceAccount.Balance < amount)
                    {
                        ModelState.AddModelError(nameof(viewModel.Amount), "You don't have enough funds to make this transfer");

                        if (!ModelState.IsValid)
                        {
                            return View(viewModel);
                        }

                        sourceAccount.Transfer(destinationAccount, amount, comment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }

                if (sourceAccount.Balance < (amount + ServiceCharge.TransferFee))
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), $"Your balance is less than requested transfer amount + withdraw fee ({ServiceCharge.TransferFee})");

                    if (!ModelState.IsValid)
                    {
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
