using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class WalletsController : Controller
    {
        private readonly List<string> curryncies = new List<string> { "Рубль", "Доллар", "Евро" };
        UserManager<User> _userManager;
        private readonly SchoolContext _context;

        public WalletsController(UserManager<User> userManager, SchoolContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            IQueryable<Wallet> wallets = _context.Wallet.Include(w => w.Firm);
            wallets = wallets.Where(p => p.Delete == false);
            wallets = wallets.Include(p => p.User);
            return View(await wallets.ToListAsync());
        }

        public async Task<IActionResult> IndexDelete()
        {
            IQueryable<Wallet> wallets = _context.Wallet.Include(w => w.Firm);
            wallets = wallets.Where(p => p.Delete == true);
            wallets = wallets.Include(p => p.User);
            return View(await wallets.ToListAsync());
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet
                .FirstOrDefaultAsync(m => m.WalletID == id);
            if (wallet == null)
            {
                return NotFound();
            }
            wallet.Firm = _context.Firm.Find(wallet.FirmID);
            wallet.User = _context.Users.Find(wallet.UserId);

            return View(wallet);
        }

        public IActionResult CreateWalletFirm()
        {
            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName");
            ViewBag.Curryncies = new SelectList(curryncies);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWalletFirm([Bind("WalletID,FirmID,UserId,WalletName,Balance,Currency,Delete")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                var firm = _context.Firm.Find(wallet.FirmID);
                wallet.WalletName = firm.FirmName + " " + wallet.Currency;
                int i = 2;
                while (_context.Wallet.FirstOrDefault(e => e.WalletName == wallet.WalletName) != null)
                {
                    wallet.WalletName = firm.FirmName + " " + wallet.Currency + " " + i;
                    i = i + 1;
                }
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName", wallet.FirmID);
            ViewBag.Curryncies = new SelectList(curryncies);
            return View(wallet);
        }


        public IActionResult CreateWalletUser(string id)
        {
            IQueryable<User> users = _context.Users;
            users = users.Where(p => p.Delete == false );
            ViewBag.Users = new SelectList(users, "Id", "UserName",id);
            ViewBag.Curryncies = new SelectList(curryncies);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWalletUser([Bind("WalletID,FirmID,UserId,WalletName,Balance,Currency,Delete")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
               var User = _context.Users.Find(wallet.UserId);
                wallet.WalletName = User.FirstName + " " + User.LastName + " " + wallet.Currency;
                int i = 2;
                while (_context.Wallet.FirstOrDefault(e => e.WalletName == wallet.WalletName) != null)
                {
                    wallet.WalletName = User.FirstName + " " + User.LastName + " " + wallet.Currency + " " + i;
                    i = i + 1;
                }
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            IQueryable<User> users = _context.Users;
            users = users.Where(p => p.Delete == false);
            ViewBag.Users = new SelectList(users, "Id", "UserName", wallet.UserId);
            ViewBag.Curryncies = new SelectList(curryncies);
            return View(wallet);
        }

        public IActionResult CreateWalletOther()
        {
            ViewBag.Curryncies = new SelectList(curryncies);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWalletOther([Bind("WalletID,FirmID,UserId,WalletName,Balance,Currency,Delete")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {

                wallet.WalletName = wallet.WalletName + " " + wallet.Currency;
                int i = 2;
                while (_context.Wallet.FirstOrDefault(e => e.WalletName == wallet.WalletName) != null)
                {
                    wallet.WalletName = wallet.WalletName + " " + wallet.Currency + " " + i;
                    i = i + 1;
                }
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Curryncies = new SelectList(curryncies);
            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewData["FirmID"] = new SelectList(_context.Firm, "FirmID", "FirmID", wallet.FirmID);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wallet.UserId);
            ViewBag.Curryncies = new SelectList(curryncies);
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WalletID,FirmID,UserId,WalletName,Balance,Currency,Delete")] Wallet wallet)
        {
            var wallet2 = _context.Wallet.Find(id);
            wallet2.Currency = wallet.Currency;
            wallet2.Balance = wallet.Balance;
            if (id != wallet.WalletID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallet2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.WalletID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirmID"] = new SelectList(_context.Firm, "FirmID", "FirmID", wallet.FirmID);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wallet.UserId);
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet
                .Include(w => w.Firm)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WalletID == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wallet = await _context.Wallet.FindAsync(id);
            wallet.Delete = true;
            _context.Wallet.Update(wallet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Undelete(int id)
        {
            var wallet = await _context.Wallet.FindAsync(id);
            wallet.Delete = false;
            _context.Wallet.Update(wallet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexDelete));
        }

        private bool WalletExists(int id)
        {
            return _context.Wallet.Any(e => e.WalletID == id);
        }
    }
}
