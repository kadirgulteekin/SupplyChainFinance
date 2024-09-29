using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class PaymentCompletedEvent
    {
        public string? InvoiceNumber { get; set; } 
        public string? SupplierTaxId { get; set; }
        public string? BuyerTaxId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; } 
    }
}
