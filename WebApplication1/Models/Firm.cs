using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Firm
    {
        public int FirmID { get; set; }
        public string FirmName { get; set; }
        public string FirmAddress { get; set; }

        public List<Wallet> Wallets { get; set; }
        public List<User> Users { get; set; }

        public bool Delete { get; set; }
    }
}
