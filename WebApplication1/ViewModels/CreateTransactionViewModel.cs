using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class CreateTransactionViewModel
    {
        public int? WalletID { get; set; }
        public int? WalletIDSender { get; set; }
        public int? WalletIDRecipient { get; set; }
        public float AmountInCurrencySender { get; set; }
        public float AmountInCurrencyRecipient { get; set; }
        public string Description { get; set; }
        public float Course { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string GetCurrency { get; set; }

        public string SenderRecipient { get; set; }
        public string WalletName { get; set; }
        public float Amount { get; set; }
    }
}
