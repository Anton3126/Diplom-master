using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime WaitDatePayment { get; set; }
        public int Amount { get; set; }
        public string ClientName { get; set; }
        public int AmountWait { get; set; }
        public int AmountReal { get; set; }
        public DateTime DatePaymentWait { get; set; }
        public DateTime DatePaymentReal { get; set; }

        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public bool Delete { get; set; }
    }
}
