using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.DAL;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly List<string> curryncies = new List<string> { "Рубль", "Доллар", "Евро" };
        UserManager<User> _userManager;
        private readonly SchoolContext _context;

        public UsersController(UserManager<User> userManager, SchoolContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            IQueryable<User> users = _userManager.Users;
            users = users.Where(p => p.Delete == false);
            users = users.Include(p => p.Firm);
            users = users.Include(p => p.Wallets);
            users = users.Include(p => p.Tasks);
            var userList = users.ToList();
            foreach (User user in userList.ToList())
            {
                foreach (Wallet wallet in user.Wallets.ToList())
                {
                    if (wallet.Delete == true)
                    {
                        user.Wallets.Remove(wallet);
                    }
                }
            }
            foreach (User user in userList.ToList())
            {
                foreach (Models.Task task in user.Tasks.ToList())
                {
                    if (task.Delete == true)
                    {
                        user.Tasks.Remove(task);
                    }
                }
            }
            return View(userList);
        }

        public IActionResult IndexDelete()
        {
            IQueryable<User> users = _userManager.Users;
            users = users.Where(p => p.Delete == true);
            users = users.Include(p => p.Firm);
            users = users.Include(p => p.Wallets);
            var userList = users.ToList();
            foreach (User user in userList.ToList())
            {
                foreach (Wallet wallet in user.Wallets.ToList())
                {
                    if (wallet.Delete == true)
                    {
                        user.Wallets.Remove(wallet);
                    }
                }
            }
            return View(userList);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.Firm = _context.Firm.Find(user.FirmID);
            return View(user);
        }

        public IActionResult Create()
        {
            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName");
            ViewBag.Curryncies = new SelectList(curryncies);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.DateImployment = DateTime.Now;
                User user = new User { Email = model.Email, UserName = model.UserName, Year = model.Year, FirmID = model.FirmID,FirstName = model.FirstName,
                    MiddleName = model.MiddleName, LastName = model.LastName, DateImployment = model.DateImployment, Delete = false };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "user");
                await _context.SaveChangesAsync();
                Wallet wallet = new Wallet { Balance = 0, Currency = model.Currency, UserId = user.Id, WalletName = user.FirstName + " " + user.LastName + " " + model.Currency };
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName", model.FirmID);
            ViewBag.Curryncies = new SelectList(curryncies);
            return View(model);
        }

        public ViewResult SomeMethod()
        {
            ViewData["Head"] = "Привет мир!";
            return View("Create");
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.UserName, Year = user.Year,
                FirmID = user.FirmID,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateImployment = user.DateImployment
            };
            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName", model.FirmID);
            return View(model);
        }   

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.Year = model.Year;
                    user.FirmID = model.FirmID;
                    user.FirstName = model.FirstName;
                    user.MiddleName = model.MiddleName;
                    user.LastName = model.LastName;
                    user.DateImployment = model.DateImployment;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName", model.FirmID);
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(w => w.Firm)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Delete = true;
                IdentityResult result = await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Undelete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Delete = false;
                IdentityResult result = await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("IndexDelete");
        }
    }
}
