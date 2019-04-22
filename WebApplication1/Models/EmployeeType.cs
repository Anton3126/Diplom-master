using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class EmployeeType
    {
        public int EmployeeTypeID { get; set; }
        public string UserId { get; set; }
        public int Task1ID { get; set; }
        public int Amount { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string PaymentType { get; set; }

        public User User { get; set; }
       // public Task1 Task1 { get; set; }

        public bool Delete { get; set; }
    }
}
