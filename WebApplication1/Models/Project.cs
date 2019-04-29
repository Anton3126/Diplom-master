using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public enum SortState
    {
        NameAsc,    // по имени по возрастанию
        NameDesc,   // по имени по убыванию        
        CostAsc, // по ожидаемой стоимости по возрастанию
        CostDesc // по ожидаемой стоимости по убыванию
    }

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
        public int ActualCost { get; set; }
        public string Status { get; set; }
        //public static List<string> Statuses = new List<string>() { "Активный", "Завершенный" };

        //Навигационное свойство для таблицы Tasks
        public List<Task> Tasks { get; set; }
        //Навигационное свойство для таблицы Invoices
        public List<Invoice> Invoices { get; set; }


        //Внешний ключ для таблица Firms
        //public int? FirmId { get; set; }
        //public Firm Firm { get; set; }

    }
}
