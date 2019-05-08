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
    public class FirmsController : Controller
    {
        private readonly List<string> curryncies = new List<string> { "Рубль", "Доллар", "Евро" };
        private readonly SchoolContext _context;

        public FirmsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Firms
        public IActionResult Index()
        {
            IQueryable<Firm> firms = _context.Firm.Include(e => e.Wallets);
            firms = firms.Where(p => p.Delete == false);
            var firmList = firms.ToList();
            foreach (Firm firm in firmList.ToList())
            {
                foreach (Wallet wallet in firm.Wallets.ToList())
                {
                    if (wallet.Delete == true)
                    {
                        firm.Wallets.Remove(wallet);
                    }
                }
            }
            return View(firmList);
        }

        public IActionResult IndexDelete()
        {
            IQueryable<Firm> firms = _context.Firm.Include(e => e.Wallets);
            firms = firms.Where(p => p.Delete == true);
            var firmList = firms.ToList();
            foreach (Firm firm in firmList.ToList())
            {
                foreach (Wallet wallet in firm.Wallets.ToList())
                {
                    if (wallet.Delete == true)
                    {
                        firm.Wallets.Remove(wallet);
                    }
                }
            }
            return View(firmList);
        }

        // GET: Firms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firm = await _context.Firm
                .FirstOrDefaultAsync(m => m.FirmID == id);
            if (firm == null)
            {
                return NotFound();
            }

            return View(firm);
        }

        // GET: Firms/Create
        public IActionResult Create()
        {
            ViewBag.Curryncies = new SelectList(curryncies);
            return View();
        }

        // POST: Firms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFirmViewModel model)
        {
            if (ModelState.IsValid)
            {
                Firm firm = new Firm
                {
                    FirmName = model.FirmName,
                    FirmAddress = model.FirmAddress,
                };
                _context.Add(firm);
                await _context.SaveChangesAsync();
                Wallet wallet = new Wallet { Balance = 0, Currency = model.Currency, FirmID = firm.FirmID, WalletName = firm.FirmName + " " + model.Currency
            };
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Curryncies = new SelectList(curryncies);
            return View(model);
        }

        // GET: Firms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firm = await _context.Firm.FindAsync(id);
            if (firm == null)
            {
                return NotFound();
            }
            return View(firm);
        }

        // POST: Firms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirmID,FirmName,FirmAddress,Delete")] Firm firm)
        {
            if (id != firm.FirmID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(firm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmExists(firm.FirmID))
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
            return View(firm);
        }

        // GET: Firms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firm = await _context.Firm
                .FirstOrDefaultAsync(m => m.FirmID == id);
            if (firm == null)
            {
                return NotFound();
            }

            return View(firm);
        }

        // POST: Firms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firm = await _context.Firm.FindAsync(id);
            firm.Delete = true;
            _context.Firm.Update(firm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Undelete(int id)
        {
            var firm = await _context.Firm.FindAsync(id);
            firm.Delete = false;
            _context.Firm.Update(firm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexDelete));
        }

        private bool FirmExists(int id)
        {
            return _context.Firm.Any(e => e.FirmID == id);
        }
    }
}
