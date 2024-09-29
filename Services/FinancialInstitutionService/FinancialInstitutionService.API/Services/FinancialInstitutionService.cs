
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Events;
using Shared.Models;
using SupplierService.Infrastructure.Data;
using System.Net.Http;
using System.Text.Json;

namespace FinancialInstitutionService.API.Services
{
    public class FinancialInstitutionService : IFinancialInstitutionService
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<FinancialInstitutionService> _logger;
        private readonly SupplierDbContext _supplierDbContext;
        public FinancialInstitutionService(ISendEndpointProvider sendEndpointProvider, ILogger<FinancialInstitutionService> logger,SupplierDbContext supplierDbContext)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _logger = logger;
            _supplierDbContext = supplierDbContext;
           
        }

        public async Task<Invoice?> GetInvoiceInfo(string invoiceNumber)
        {
            return await _supplierDbContext.Invoices
                .Where(i => i.InvoiceNumber == invoiceNumber)
                .FirstOrDefaultAsync();
        }

        public async Task ProcessPayment(string invoiceNumber, decimal paymentAmount)
        {
            using var transaction = await _supplierDbContext.Database.BeginTransactionAsync();
            try
            {
   
                var invoiceInfo = await GetInvoiceInfo(invoiceNumber);

                if (invoiceInfo == null)
                {
                    _logger.LogError($"No invoice found with InvoiceNumber: {invoiceNumber}");
                    throw new Exception($"Invoice not found: {invoiceNumber}");
                }


                var paymentCompletedEvent = new PaymentCompletedEvent
                {
                    InvoiceNumber = invoiceNumber,
                    SupplierTaxId = invoiceInfo.SupplierTaxId,
                    BuyerTaxId = invoiceInfo.BuyerTaxId,
                    PaymentAmount = paymentAmount,
                    PaymentDate = DateTime.UtcNow
                };

               
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment-completed-event"));
                await sendEndpoint.Send(paymentCompletedEvent);
                await transaction.CommitAsync();

                _logger.LogInformation($"PaymentCompletedEvent sent for InvoiceNumber: {invoiceNumber}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"An error occurred while processing payment for InvoiceNumber: {invoiceNumber}. Error: {ex.Message}");
                throw;
            }
        }
    }
}
