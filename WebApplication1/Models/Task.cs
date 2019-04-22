using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<UserTask> UserTasks { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
