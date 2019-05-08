using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        public int? Year { get; set; }
        public int? FirmID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateImployment { get; set; }
        public int? Percent { get; set; }
        public string Post { get; set; }

        public Firm Firm { get; set; }

        public List<Wallet> Wallets { get; set; }
        public List<EmployeeType> EmployeeTypes { get; set; }
        public List<Task> Tasks { get; set; }

        public bool Delete { get; set; }
    }
}
