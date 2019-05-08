using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class SotrudnikDetailsController : Controller
    {
        private readonly SchoolContext _context;

        public SotrudnikDetailsController(SchoolContext context)
        {
            _context = context;
        }

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
    }
}