using BuyerService.Infrastructure.Data;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Events;
using Shared.Models;

namespace BuyerService.API.Services
{
    public class BuyerManageService : IBuyerManageService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpoint;

        public BuyerManageService(ApplicationDbContext applicationDbContext, IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpoint)
        {
            _applicationDbContext = applicationDbContext;
            _publishEndpoint = publishEndpoint;
            _sendEndpoint = sendEndpoint;
        }

        public async Task<ResponseDto<Invoice>> UploadInvoice(Invoice invoice)
        {
            using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();
            if (invoice == null)
            {
                throw new ArgumentException("Invalid invoice data");
            }

            try
            {
                invoice.Status = InvoiceStatus.New;
                _applicationDbContext.Invoices.Add(invoice);
                await _applicationDbContext.SaveChangesAsync();


                var invoiceItem = new InvoiceUploadedEvent
                {
                    InvoiceNumber = invoice.InvoiceNumber,
                    SupplierTaxId = invoice.SupplierTaxId,
                    BuyerTaxId = invoice.BuyerTaxId,
                    InvoiceCost = invoice.InvoiceCost,
                    TermDate = invoice.TermDate,
                    StatusType = invoice.Status switch
                    {
                        InvoiceStatus.New => InvoiceStatus.New,
                        InvoiceStatus.Used => InvoiceStatus.Used,
                        InvoiceStatus.Paid => InvoiceStatus.Paid,
                        _ => throw new ArgumentOutOfRangeException($"Unexpected status: {invoice.Status}")
                    }
                };
            

                var sendEndpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:{"invoice-uploaded-event"}"));
                await sendEndpoint.Send<InvoiceUploadedEvent>(invoiceItem);
                await transaction.CommitAsync();
                return Shared.Dtos.ResponseDto<Invoice>.Success(invoice,StatusCodes.Status202Accepted);
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync(); // Hata durumunda rollback
                throw new ApplicationException("An error occurred while processing the invoice.", ex);
            }
        }
    }
}
