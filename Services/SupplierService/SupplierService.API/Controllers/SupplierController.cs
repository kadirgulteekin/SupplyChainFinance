using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ControllerBases;
using Shared.Models;
using SupplierService.API.Services;

namespace SupplierService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : CustomBaseController
    {
        private readonly ISupplierServices _supplierServices;

        public SupplierController(ISupplierServices supplierServices)
        {
            _supplierServices = supplierServices;

        }

        [HttpGet("invoices/{supplierTaxId}")]
        public async Task<IActionResult> GetInvoicesBySupplierId(string supplierTaxId)
        {
            var invoices = await _supplierServices.GetInvoicesBySupplierIdAsync(supplierTaxId);
            return Ok(invoices);
        }


        [HttpPost("invoices/{invoiceId}/early-payment")]
        public async Task<IActionResult> RequestEarlyPayment(string invoiceId)
        {
            var invoice = await _supplierServices.GetInvoicesBySupplierIdAsync(invoiceId);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }

            // Fatura durumunu kontrol et
            if (invoice.Data.Status == InvoiceStatus.Used || invoice.Data.Status == InvoiceStatus.Paid)
            {
                return BadRequest("Early payment has already been requested or invoice is paid.");
            }

            var response = await _supplierServices.RequestEarlyPaymentAsync(invoiceId);
            if (response.IsSuccessful)
            {
                return NotFound(response.Errors);
            }

            return CreateActionResultInstance(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterSupplier(Supplier supplier)
        {
            var result = await _supplierServices.AddSupplierAsync(supplier);

            return CreateActionResultInstance(result);
        }
    }
}
