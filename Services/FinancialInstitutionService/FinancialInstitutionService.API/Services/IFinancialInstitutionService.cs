using Shared.Dtos;
using Shared.Models;

namespace FinancialInstitutionService.API.Services
{
    public interface IFinancialInstitutionService
    {
        Task ProcessPayment(string invoiceNumber, decimal paymentAmount);
        Task<Invoice?> GetInvoiceInfo(string invoiceNumber);

    }
}
