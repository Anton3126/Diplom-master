using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
        public bool Status { get; set; }

        //Навигационное свойство для таблицы Tasks
        public List<Task> Tasks { get; set; }
        //Навигационное свойство для таблицы Invoices
        public List<Invoice> Invoices { get; set; }


        //Внешний ключ для таблица Firms
        //public int? FirmId { get; set; }
        //public Firm Firm { get; set; }

    }
}
