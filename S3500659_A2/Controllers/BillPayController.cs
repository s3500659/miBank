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

    }
}
