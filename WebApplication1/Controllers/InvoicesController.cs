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

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class InvoicesController : Controller
    {
        private readonly List<string> pay = new List<string> { "Неоплачен", "Оплачен" };
        private readonly SchoolContext _context;

        public InvoicesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public IActionResult Index()
        {
            IQueryable<Invoice> invoices = _context.Invoice;
            invoices = invoices.Where(p => p.Delete == false);
            invoices = invoices.Include(p => p.Project);
            return View(invoices);
        }

        public IActionResult IndexDelete()
        {
            IQueryable<Invoice> invoices = _context.Invoice;
            invoices = invoices.Where(p => p.Delete == true);
            invoices = invoices.Include(p => p.Project);
            return View(invoices);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Name");
            ViewBag.Pay = new SelectList(pay);
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceID,ClientName,DateCreate,AmountWait,AmountReal,DatePaymentWait,DatePaymentReal,Description,ProjectID,Delete")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.DateCreate = DateTime.Now;
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Name", invoice.ProjectID);
            ViewBag.Pay = new SelectList(pay);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Name", invoice.ProjectID);
            ViewBag.Pay = new SelectList(pay);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceID,ClientName,DateCreate,AmountWait,AmountReal,DatePaymentWait,DatePaymentReal,Description,ProjectID,Delete")] Invoice invoice)
        {
            if (id != invoice.InvoiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceID))
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
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Id", invoice.ProjectID);
            ViewBag.Pay = new SelectList(pay);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            invoice.Delete = true;
            _context.Invoice.Update(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Undelete(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            invoice.Delete = false;
            _context.Invoice.Update(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexDelete));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.InvoiceID == id);
        }
    }
}
