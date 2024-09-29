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

        public PaymentCompletedConsumer(SupplierDbContext supplierDbContext, ISupplierServices supplierServices)
        {
            _supplierDbContext = supplierDbContext;
            _supplierServices = supplierServices;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var paymentEvent = context.Message;

            // Ödeme başarıyla yapıldıysa faturanın durumunu Paid yap
            var invoice = await _supplierServices.GetInvoicesBySupplierIdAsync(paymentEvent.InvoiceNumber);
            invoice.Data.Status = InvoiceStatus.Paid;
            await _supplierDbContext.Invoices.AddAsync(invoice.Data);

        }
    }
}
