using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
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
            Wallet walletSender = _context.Wallet.Find(id);
            Wallet walletRecipient = _context.Wallet.Find(id+1);
            if (walletSender == null)
            {
                return NotFound();
            }
            CreateTransactionViewModel model = new CreateTransactionViewModel
            {
                WalletNameSender = walletSender.WalletName,
                WalletNameRecipient = walletRecipient.WalletName,
                Currency = walletSender.Currency
            };

            IQueryable<Wallet> wallets = _context.Wallet;
            wallets = wallets.Where(p => p.Delete == false);
            wallets = wallets.Include(p => p.Firm);
            wallets = wallets.Include(p => p.User);
            // ViewBag.Wallet = new SelectList(wallets, "WalletID", "WalletID");

            ViewData["RelatedTransactionID"] = new SelectList(_context.Transaction, "TransactionID", "TransactionID");
            ViewBag.WalletSender = new SelectList(wallets, "WalletName", "WalletName");
            ViewBag.WalletRecipient = new SelectList(wallets, "WalletName", "WalletName");

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
                var walletSender = await _context.Wallet.FindAsync(model.WalletNameSender);
                var walletRecipient = await _context.Wallet.FindAsync(model.WalletNameRecipient);
                Transaction transactionSender = new Transaction
                {
                    WalletID = walletSender.WalletID,
                    Type = model.Type,
                    PlusMinus = '-',
                    Amount = model.AmountInCurrencySender,
                    AmountInCurrencySender = model.AmountInCurrencySender,
                    AmountInCurrencyRecipient = model.AmountInCurrencyRecipient,
                    Date = DateTime.Now,
                    Description = model.Description,
                    Course = model.Course
                };
                _context.Add(transactionSender);
                await _context.SaveChangesAsync();

                Transaction transactionRecipient = new Transaction
                {
                    WalletID = walletRecipient.WalletID,
                    Type = model.Type,
                    PlusMinus = '+',
                    Amount = model.AmountInCurrencyRecipient,
                    AmountInCurrencySender = model.AmountInCurrencySender,
                    AmountInCurrencyRecipient = model.AmountInCurrencyRecipient,
                    Date = DateTime.Now,
                    Description = model.Description,
                    Course = model.Course,
                    RelatedTransactionID = transactionSender.TransactionID
                    
                };
                _context.Add(transactionRecipient);
                await _context.SaveChangesAsync();

                transactionSender.RelatedTransactionID = transactionRecipient.TransactionID;
                _context.Update(transactionSender);
                await _context.SaveChangesAsync();

                walletSender.Balance = walletSender.Balance - model.AmountInCurrencySender;
                _context.Update(walletSender);
                await _context.SaveChangesAsync();

                walletRecipient.Balance = walletRecipient.Balance + (model.AmountInCurrencyRecipient);
                _context.Update(walletRecipient);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            IQueryable<Wallet> wallets = _context.Wallet;
            wallets = wallets.Where(p => p.Delete == false);
            ViewBag.WalletSender = new SelectList(wallets, "WalletName", "WalletNameSender", model.WalletIDSender);
            ViewBag.WalletRecipient = new SelectList(wallets, "WalletName", "WalletNameRecipient", model.WalletIDRecipient);

            ViewBag.Types = new SelectList(types);
            return View(model);
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionID == id);
        }
    }
}
