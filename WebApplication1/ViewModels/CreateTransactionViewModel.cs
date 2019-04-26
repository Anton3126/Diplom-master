using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CreateTransactionViewModel
    {
        public int? WalletIDSender { get; set; }
        public int? WalletIDRecipient { get; set; }
        public float AmountInCurrencySender { get; set; }
        public float AmountInCurrencyRecipient { get; set; }
        public string Description { get; set; }
        public float Course { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string GetCurrency { get; set; }

        public string WalletNameSender { get; set; }
        public string WalletNameRecipient { get; set; }
    }
}
