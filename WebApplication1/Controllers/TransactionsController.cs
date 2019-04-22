using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly List<string> types = new List<string> { "Налоги", "Аренда", "Зарплата", "Другое" };
        private readonly SchoolContext _context;

        public TransactionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Transaction.Include(t => t.RelatedTransaction).Include(u => u.Wallet.User).Include(f => f.Wallet.Firm).Include(w => w.Wallet);
            return View(await schoolContext.ToListAsync());
        }

        public async Task<IActionResult> IndexIndividual(int id)
        {
            IQueryable<Transaction> transactions = _context.Transaction;
            transactions = transactions.Where(t => t.WalletID == id);
            transactions = transactions.Include(u => u.Wallet.User);
            transactions = transactions.Include(f => f.Wallet.Firm);
            transactions = transactions.Include(t => t.RelatedTransaction);
            transactions = transactions.Include(w => w.Wallet);

            /*var wallet = _context.Wallet.Find(id);
              if (wallet != null)
              {
                if (wallet.FirmID != null) { ViewData["Head"] = wallet.Firm.FirmName; }
                if (wallet.UserId != null) { ViewData["Head"] = wallet.User.FirstName + " " + wallet.User.LastName; }
                if (wallet.WalletName != null) { ViewData["Head"] = wallet.WalletName; }
              } */
            return View(await transactions.ToListAsync());
        }

        // GET: Transactions/Create
        public IActionResult Create(int? id)
        {
            Wallet wallet = _context.Wallet.Find(id);
            if (wallet == null)
            {
                return NotFound();
            }
            CreateTransactionViewModel model = new CreateTransactionViewModel
            {
                WalletID = wallet.WalletID,
                Currency = wallet.Currency
            };

            IQueryable<Wallet> wallets = _context.Wallet;
            wallets = wallets.Where(p => p.Delete == false);
            ViewBag.Wallet = new SelectList(wallets, "WalletID", "WalletID");

            ViewData["RelatedTransactionID"] = new SelectList(_context.Transaction, "TransactionID", "TransactionID");
            ViewData["WalletID"] = new SelectList(_context.Wallet, "WalletID", "WalletID");

            ViewData["Types"] = new SelectList(types);
            return View(model);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Transaction transaction1 = new Transaction
                {
                    WalletID = model.WalletID,
                    Type = model.Type,
                    TypePM = '-',
                    Amount1 = model.Amount1,
                    Amount2 = model.Amount2,
                    Date = DateTime.Now,
                    Description = model.Description,
                    Course = model.Course
                };
                _context.Update(transaction1);
                await _context.SaveChangesAsync();

                Transaction transaction2 = new Transaction
                {
                    WalletID = model.GetWalletID,
                    Type = model.Type,
                    TypePM = '+',
                    Amount1 = model.Amount1,
                    Amount2 = model.Amount2,
                    Date = DateTime.Now,
                    Description = model.Description,
                    Course = model.Course,
                    RelatedTransactionID = transaction1.TransactionID
                    
                };
                _context.Update(transaction2);
                await _context.SaveChangesAsync();

                transaction1.RelatedTransactionID = transaction2.TransactionID;
                _context.Update(transaction1);
                await _context.SaveChangesAsync();

                Wallet wallet1 = _context.Wallet.Find(model.WalletID);
                wallet1.Balance = wallet1.Balance - model.Amount1;
                _context.Update(wallet1);
                await _context.SaveChangesAsync();

                Wallet wallet2 = _context.Wallet.Find(model.GetWalletID);
                wallet2.Balance = wallet2.Balance + (model.Amount2);
                _context.Update(wallet2);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            IQueryable<Wallet> wallets = _context.Wallet;
            wallets = wallets.Where(p => p.Delete == false);
            ViewBag.Wallet = new SelectList(wallets, "WalletID", "WalletID", model.WalletID);

            ViewBag.Types = new SelectList(types);
            return View(model);
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionID == id);
        }
    }
}
