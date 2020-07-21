using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3500659_A2.Data;
using S3500659_A2.Models;
using SimpleHashing;

namespace S3500659_A2.Controllers
{
    public class LoginController : Controller
    {
        private readonly A2Context _context;

        public LoginController(A2Context context) => _context = context;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string loginID, string password)
        {
            /*
             * Getting the login ID from the input of the web app
             */
            var login = await _context.Logins.FindAsync(loginID);

            /*
             * Verify password with simple hashing
             */
            if (login == null || !PBKDF2.Verify(login.Password, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { LoginID = loginID });
            }

            // Login customer.
            /*
             * Setting the session variable
             */
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), login.Customer.CustomerName);

            /*
             * send sessions variable to the customer controller, index method
             */
            return RedirectToAction("index", "ATM");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            /*
             * You need to clear the session variable when you log out
             */
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
