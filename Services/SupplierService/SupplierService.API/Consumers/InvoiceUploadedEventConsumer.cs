using MassTransit;
using Shared.Events;
using Shared.Models;
using SupplierService.API.Services;
using SupplierService.Infrastructure.Data;

namespace SupplierService.API.Consumers
{
    public class InvoiceUploadedEventConsumer : IConsumer<InvoiceUploadedEvent>
    {
        private readonly ILogger<InvoiceUploadedEventConsumer> _logger;
        private readonly ISupplierServices _supplierServices;
        private readonly SupplierDbContext _supplierDbContext;



        public InvoiceUploadedEventConsumer(ILogger<InvoiceUploadedEventConsumer> logger,
                                       ISupplierServices supplierServices,SupplierDbContext supplierDbContext
                                      )
        {
            _logger = logger;
            _supplierServices = supplierServices;
            _supplierDbContext = supplierDbContext;


        }

        public async Task Consume(ConsumeContext<InvoiceUploadedEvent> context)
        {
            var invoiceEvent = context.Message;

           
            _logger.LogInformation($"Invoice uploaded: {invoiceEvent.InvoiceNumber} by Buyer: {invoiceEvent.BuyerTaxId} for Supplier: {invoiceEvent.SupplierTaxId}");

            try
            {
                // Log öncesi
                _logger.LogInformation("Fetching supplier from database...");

                // Supplier'ı veritabanından al
                var supplier = await _supplierServices.GetSupplierByTaxIdAsync(invoiceEvent.SupplierTaxId);

                // Log sonrası
                _logger.LogInformation("Supplier fetched successfully");

                if (supplier == null)
                {
                    _logger.LogError($"Supplier not found for Tax ID: {invoiceEvent.SupplierTaxId}");
                    throw new Exception($"Supplier with Tax ID {invoiceEvent.SupplierTaxId} not found.");
                }


                if (!Enum.IsDefined(typeof(InvoiceStatus), invoiceEvent.StatusType))
                {
                    _logger.LogError($"Invalid status type: {invoiceEvent.StatusType}");
                    throw new ArgumentException($"Invalid status type: {invoiceEvent.StatusType}");
                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error occurred: {ex.Message}");
                throw;
            }
         

            var invoice = new Invoice
            {
                InvoiceNumber = invoiceEvent.InvoiceNumber,
                BuyerTaxId = invoiceEvent.BuyerTaxId,
                SupplierTaxId = invoiceEvent.SupplierTaxId,
                InvoiceCost = invoiceEvent.InvoiceCost,
                TermDate = invoiceEvent.TermDate,
                Status = (InvoiceStatus)invoiceEvent.StatusType
            };

            using var transaction = await _supplierDbContext.Database.BeginTransactionAsync();

            try
            {
                await _supplierDbContext.Invoices.AddAsync(invoice);
                await _supplierDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation($"Invoice {invoiceEvent.InvoiceNumber} saved successfully for Supplier: {invoiceEvent.SupplierTaxId}");
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                _logger.LogError($"Error saving invoice {invoiceEvent.InvoiceNumber} for Supplier: {invoiceEvent.SupplierTaxId}. Error: {ex.Message}");
                throw;
            }


    

            _logger.LogInformation($"Invoice {invoiceEvent.InvoiceNumber} saved successfully for Supplier: {invoiceEvent.SupplierTaxId}");
        }
    }
}
