using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication1.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int? RelatedTransactionID { get; set; }
        public int? WalletID { get; set; }
        public string Type { get; set; }
        public char PlusMinus { get; set; }
        public float Amount { get; set; }
        public float AmountInCurrencySender { get; set; }
        public float AmountInCurrencyRecipient { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public float Course { get; set; }

        public Wallet Wallet { get; set; }
        public Transaction RelatedTransaction { get; set; }

        public bool Delete { get; set; }
    }
}
