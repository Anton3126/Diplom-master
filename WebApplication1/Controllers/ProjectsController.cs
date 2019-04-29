using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.DAL;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProjectsController : Controller
    {
        List<String> statusList = new List<string>() {"Все", "Активный", "Завершенный" };
        private readonly SchoolContext _context;

        public ProjectsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index( string status, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Project> projects = _context.Projects;

            if (status == "Все")
            {
                projects = projects.Where(p => p.Status != null);
            }
            else if (status != null && status != "")
            {
                projects = projects.Where(p => p.Status == status);
            }

            ViewData["Status"] = statusList.Select(m => new SelectListItem { Text = m, Value = m });

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["CostSort"] = sortOrder == SortState.CostAsc ? SortState.CostDesc : SortState.CostAsc;

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case SortState.CostAsc:
                    projects = projects.OrderBy(s => s.Cost);
                    break;
                case SortState.CostDesc:
                    projects = projects.OrderByDescending(s => s.Cost);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            return View(await projects.AsNoTracking().ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Date,Cost")] Project project)
        {
            project.Status = "Активный";
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }           

            ViewData["Status"] = statusList.Select(m => new SelectListItem { Text = m, Value = m});

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Date,Cost")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
