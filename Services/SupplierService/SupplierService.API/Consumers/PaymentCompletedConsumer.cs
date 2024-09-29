using BuyerService.Domain.Model;
using MassTransit;
using Shared.Events;
using Shared.Models;
using SupplierService.API.Services;
using SupplierService.Infrastructure.Data;

namespace SupplierService.API.Consumers
{
    public class PaymentCompletedConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly SupplierDbContext _supplierDbContext;
        private readonly ISupplierServices _supplierServices;
        private readonly ILogger<PaymentCompletedConsumer> _logger;

        public PaymentCompletedConsumer(SupplierDbContext supplierDbContext, ISupplierServices supplierServices, ILogger<PaymentCompletedConsumer> logger)
        {
            _supplierDbContext = supplierDbContext;
            _supplierServices = supplierServices;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var paymentEvent = context.Message;
            try
            {
                var invoice = await _supplierServices.GetInvoicesBySupplierIdAsync(paymentEvent.InvoiceNumber);

                if (invoice == null)
                {
                    _logger.LogError($"Invoice not found: {paymentEvent.InvoiceNumber}");
                    throw new Exception($"Invoice {paymentEvent.InvoiceNumber} not found.");
                }
                invoice.Data.Status = Shared.Models.InvoiceStatus.Paid;
                invoice.Data.InvoiceCost = context.Message.PaymentAmount;
                await _supplierDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error saving invoice {paymentEvent.InvoiceNumber} for Supplier: {paymentEvent.SupplierTaxId}. Error: {ex.Message}");
            }
           

            
          

        }
    }
}
