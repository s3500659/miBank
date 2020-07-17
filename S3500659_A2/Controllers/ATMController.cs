using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NwbaExample.Utilities;
using S3500659_A2.Data;
using S3500659_A2.Filters;
using S3500659_A2.Models;

namespace S3500659_A2.Controllers
{
    [AuthorizeCustomer]
    public class ATMController : Controller
    {
        private readonly A2Context _context;

        // retrieve the session variable
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public ATMController(A2Context context)
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

                        ViewBag.Amount = amount;
                        return View(account);

                }
                else
                {
                    account.Withdraw(amount, comment, ServiceCharge.WithdrawFee);
                }
            }

            

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
