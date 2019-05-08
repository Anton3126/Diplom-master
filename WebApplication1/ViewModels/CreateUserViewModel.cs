using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Year { get; set; }
        public int? FirmID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateImployment { get; set; }
        public string Currency { get; set; }
        public int? Percent { get; set; }
        public string Post { get; set; }

        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public CreateUserViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
