using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using S3500659_A2.Data;
using S3500659_A2.Models;
using S3500659_A2.ViewModel;
using X.PagedList;

namespace S3500659_A2.Controllers
{
    public class BillPayController : Controller
    {
        private readonly DBContext _context;

        public BillPayController(DBContext context)
        {
            _context = context;
        }

        // GET: BillPays
        public async Task<IActionResult> Index()
        {
            var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var customer = await _context.Customers.FindAsync(customerID);
            var payees = await _context.Payees.ToListAsync();

            var model = new BillPayViewModel()
            {
                Customer = customer,
                AccountList = new SelectList(customer.Accounts, "AccountNumber", "AccountNumber"),
                PayeeList = new SelectList(payees, "PayeeID", "PayeeName")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBillPay(BillPayViewModel vm)
        {

            var period = Int32.Parse(vm.Period);
            if (ModelState.IsValid)
            {
                var billPay = new BillPay
                {
                    Account = await _context.Accounts.FindAsync(vm.AccountNumber),
                    Payee = await _context.Payees.FindAsync(vm.PayeeId),
                    Amount = vm.Amount,
                    ScheduleDate = vm.SecheduledDate,
                    ModifyDate = DateTime.UtcNow,
                    Period = GetPeriod(vm.Period)
                };
                await _context.BillPays.AddAsync(billPay);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ScheduledBillPay));
            }

            return View(vm);
        }

        public async Task<IActionResult> ScheduledBillPay()
        {
            var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var customer = await _context.Customers.FindAsync(customerID);

            var viewModel = new ScheduledPaymentViewModel()
            {
                Customer = customer,
                BillPays = await _context.BillPays.ToListAsync()
            };

            return View(viewModel);

        }

        public async Task<IActionResult> ModifyBillPay(int id)
        {
            var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var customer = await _context.Customers.FindAsync(customerID);
            var payees = await _context.Payees.ToListAsync();

            var model = new BillPayViewModel()
            {
                Customer = customer,
                AccountList = new SelectList(customer.Accounts, "AccountNumber", "AccountNumber"),
                PayeeList = new SelectList(payees, "PayeeID", "PayeeName"),
                EditId = id
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ModifyBillPay(BillPayViewModel viewModel)
        {
            var billPay = await _context.BillPays.FindAsync(viewModel.EditId);

            var period = Int32.Parse(viewModel.Period);
            if (ModelState.IsValid)
            {
                billPay.Account.AccountNumber = viewModel.AccountNumber;
                billPay.Payee.PayeeID = viewModel.PayeeId;
                billPay.Amount = viewModel.Amount;
                billPay.ScheduleDate = viewModel.SecheduledDate;
                billPay.Period = GetPeriod(viewModel.Period);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ScheduledBillPay));
            }

            return View(viewModel);
        }

        private Period GetPeriod(string period)
        {
            var intPeriod = Int32.Parse(period);

            return intPeriod == 0 ? Period.Minute
                    : (intPeriod == 1 ? Period.Quarterly
                    : (intPeriod == 2 ? Period.Annually
                    : Period.OnceOff));
        }

    }
}
