using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3500659_A2.Data;
using S3500659_A2.Models;

namespace S3500659_A2.Controllers
{
    public class StatementController : Controller
    {
        private readonly A2Context _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public StatementController(A2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            return View(account);
        }
    }
}
