using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class PaymentRequestDto
    {
        public string? InvoiceNumber { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
