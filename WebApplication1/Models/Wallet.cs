using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{

    public class Wallet
    {
        public int WalletID { get; set; }
        public int? FirmID { get; set; }
        public string UserId { get; set; }
        public string WalletName { get; set; }
        public float Balance { get; set; }
        public string Currency { get; set; }

        public Firm Firm { get; set; }
        public User User { get; set; }

        public List<Transaction> Transactions { get; set; }

        public bool Delete { get; set; }
    }
}
