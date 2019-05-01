using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.DAL;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProjectTasksController : Controller
    {
        private SchoolContext _context;
        public ProjectTasksController (SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? project)
        {
            IQueryable<Models.Task> tasks = _context.Tasks.Include(p => p.Project);
            //Вытягиваем все задачи по выбранному проекту
            if (project != null && project != 0)
            {
                tasks = tasks.Where(p => p.ProjectId == project);
            }
            List<User> users = _context.Users.ToList();
            List<Project> projects = _context.Projects.ToList();
            projects.Insert(0, new Project { Name = "Все", Id = 0 });

            ProjectTasksViewModel viewModel = new ProjectTasksViewModel
            {
                Tasks = tasks.ToList(),
                Projects = new SelectList(projects, "Id", "Name")
            };

            return View(viewModel);
        }
    }
}