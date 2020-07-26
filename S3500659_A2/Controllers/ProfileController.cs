using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S3500659_A2.Data;
using S3500659_A2.Models;
using S3500659_A2.ViewModel;
using SQLitePCL;

namespace S3500659_A2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DBContext _context;

        public ProfileController(DBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var customer = await _context.Customers.FindAsync(customerID);
            var vm = new ProfileViewModel()
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                TFN = customer.TFN,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                PostCode = customer.PostCode,
                Phone = customer.Phone
            };
            return View(vm);
        }

        public async Task<IActionResult> Edit()
        {
            var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var customer = await _context.Customers.FindAsync(customerID);

            var vm = new ProfileViewModel()
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                TFN = customer.TFN,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                PostCode = customer.PostCode,
                Phone = customer.Phone
            };
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("CustomerName, TFN, Address, City, State, PostCode, Phone")] ProfileViewModel vm)
        {
            var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var c = await _context.Customers.FindAsync(customerID);


            if (ModelState.IsValid)
            {
                c.CustomerName = vm.CustomerName;
                c.TFN = vm.TFN;
                c.Address = vm.Address;
                c.City = vm.City;
                c.State = vm.State;
                c.PostCode = vm.PostCode;
                c.Phone = vm.Phone;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                    
            }

            return View(vm);
        }

        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();

            return View(model);



        }

        //[HttpPost]
        //public async Task<IActionResult> ChangePassword()
        //{
        //    var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        //    var login = await _context.Logins.Where(x => x.CustomerID == customerID).FirstOrDefaultAsync();

        //    var model = new ChangePasswordViewModel();

        //    return View(model);



        //}
    }
}
