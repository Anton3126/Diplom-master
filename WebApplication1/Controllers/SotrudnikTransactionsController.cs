using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "user")]
    public class SotrudnikTransactionsController : Controller
    {
        private readonly SchoolContext _context;

        public SotrudnikTransactionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: SotrudnilkTransactions
        public async Task<IActionResult> Index()
        {
            IQueryable<Transaction> transactions = _context.Transaction;
            transactions = transactions.Include(w => w.Wallet);
            transactions = transactions.Where(p => p.Wallet.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);
            transactions = transactions.Include(u => u.Wallet.User);
            transactions = transactions.Include(f => f.Wallet.Firm);
            transactions = transactions.Include(t => t.RelatedTransaction);
            return View(await transactions.ToListAsync());
        }
    }
}
