using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class UserTask
    {
        public int? Hours { get; set; }

        //Связь с таблицей Users
        public string UserId { get; set; }
        public User User { get; set; }

        //Связь с таблицей Tasks
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
