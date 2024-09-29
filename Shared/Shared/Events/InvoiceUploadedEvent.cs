using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class InvoiceUploadedEvent
    {
        public string? InvoiceNumber { get; set; }  
        public string? BuyerTaxId { get; set; }     
        public string? SupplierTaxId { get; set; } 
        public decimal InvoiceCost { get; set; } 
        public DateTime TermDate { get; set; }  
        public StatusType StatusType { get; set; }
    }

    public enum StatusType
    {
        New = 0,
        Used = 1,
        Paid = 2
    }
}
