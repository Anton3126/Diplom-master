using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CreateTransactionViewModel
    {
        public int? WalletID { get; set; }
        public int? GetWalletID { get; set; }
        public float Amount1 { get; set; }
        public float Amount2 { get; set; }
        public string Description { get; set; }
        public float Course { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string GetCurrency { get; set; }
    }
}
