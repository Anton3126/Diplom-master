using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class ProjectTasksViewModel
    {
        public IEnumerable<Models.Task> Tasks { get; set; }
        public SelectList Projects { get; set; }        
    }
}
