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
    public class SotrudnikProjectsController : Controller
    {
        List<String> statusList = new List<string>() { "Все", "Активный", "Завершенный" };
        private readonly SchoolContext _context;

        public SotrudnikProjectsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Projects
        public IActionResult Index(string status, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Project> projects = _context.Projects;
            projects = projects.Include(e => e.Tasks);


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

            List<Project> projectsList = new List<Project> { };
            foreach (Project project in projects.ToList())
            {
                    foreach (Models.Task task in project.Tasks.ToList())
                    {
                        if (task.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                        {
                            projectsList.Add(project);
                        }
                    }
            }

            return View(projectsList);
        }
    }
}
