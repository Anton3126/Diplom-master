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
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class UsersController : Controller
    {
        private readonly List<string> curryncies = new List<string> { "Рубль", "Доллар", "Евро" };
        private readonly List<string> posts = new List<string> { "Программист", "Агент", "Партнер" };
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolContext _context;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SchoolContext context )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Roles = "admin")]
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
                    else
                    {
                        task.Project = _context.Projects.Find(task.ProjectId);
                    }
                }
            }
            return View(userList);
        }

        [Authorize(Roles = "admin")]
        public IActionResult IndexDelete()
        {
            IQueryable<User> users = _userManager.Users;
            users = users.Where(p => p.Delete == true);
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
                    else
                    {
                        task.Project = _context.Projects.Find(task.ProjectId);
                    }
                }
            }
            return View(userList);
        }

        [Authorize(Roles = "admin")]
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
            var wallets = _context.Wallet.ToList();
            foreach (Wallet wallet in wallets.ToList())
            {
                if (wallet.Delete == false & wallet.UserId != id)
                {
                    wallets.Remove(wallet);
                }
            }
            user.Wallets = wallets;
            var tasks = _context.Tasks.ToList();
            foreach (Models.Task task in tasks.ToList())
            {
                task.Project = _context.Projects.Find(task.ProjectId);
                if (task.Delete == false & task.UserId != id)
                {
                    tasks.Remove(task);
                }
            }
            user.Tasks = tasks;
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.ToList();
            CreateUserViewModel model = new CreateUserViewModel  { UserRoles = null, AllRoles = allRoles };

            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName");

            ViewBag.Curryncies = new SelectList(curryncies);
            ViewBag.Post = new SelectList(posts);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model, List<string> roles)
        {
            if (ModelState.IsValid)
            {
                model.DateImployment = DateTime.Now;
                User user = new User { Email = model.Email, UserName = model.UserName, Year = model.Year, FirmID = model.FirmID,FirstName = model.FirstName,
                    MiddleName = model.MiddleName, LastName = model.LastName, DateImployment = DateTime.Now, Delete = false, Percent = model.Percent, Post = model.Post};
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRolesAsync(user, roles);
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
            ViewBag.Post = new SelectList(posts);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, UserName = user.UserName, Year = user.Year,
                FirmID = user.FirmID,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateImployment = user.DateImployment,
                UserRoles = userRoles,
                AllRoles = allRoles,
                Percent = user.Percent,
                Post = user.Post
            };
            IQueryable<Firm> firms = _context.Firm;
            firms = firms.Where(p => p.Delete == false);
            ViewBag.Firms = new SelectList(firms, "FirmID", "FirmName", model.FirmID);
            ViewBag.Post = new SelectList(posts);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model, List<string> roles)
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
                    user.Percent = model.Percent;
                    user.Post = model.Post;

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
            ViewBag.Post = new SelectList(posts);
            return View(model);
        }

        [Authorize(Roles = "admin")]
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
            var wallets = _context.Wallet.ToList();
            foreach (Wallet wallet in wallets.ToList())
            {
                if (wallet.Delete == false & wallet.UserId != id)
                {
                    wallets.Remove(wallet);
                }
            }
            user.Wallets = wallets;
            var tasks = _context.Tasks.ToList();
            foreach (Models.Task task in tasks.ToList())
            {
                task.Project = _context.Projects.Find(task.ProjectId);
                if (task.Delete == false & task.UserId != id)
                {
                    tasks.Remove(task);
                }
            }
            user.Tasks = tasks;

            return View(user);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, UserName = user.UserName };
            return View(model);
        }

        [Authorize(Roles = "user, admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
