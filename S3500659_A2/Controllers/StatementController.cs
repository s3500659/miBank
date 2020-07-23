using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using S3500659_A2.Data;
using S3500659_A2.Models;
using X.PagedList;

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

        public async Task<IActionResult> Index(int id, int? page = 1)
        {
            var account = await _context.Accounts.FindAsync(id);

            var transactions = account.Transactions.OrderByDescending(x => x.ModifyDate);

            const int pageSize = 4;

            var pagedList = transactions.ToPagedList((int)page, pageSize);

            return View(pagedList);
        }
    }
}
