using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public string ClientName { get; set; }
        public DateTime DateCreate { get; set; }
        public float? AmountWait { get; set; }
        public float? AmountReal { get; set; }
        public DateTime? DatePaymentWait { get; set; }
        public DateTime? DatePaymentReal { get; set; }
        public string Description { get; set; }
        public string Pay { get; set; }

        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public bool Delete { get; set; }
    }
}
