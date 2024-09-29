using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Models;
using System;

namespace SupplierService.API.Services
{
    public interface ISupplierServices
    {
        Task<Supplier> GetSupplierByTaxIdAsync(string taxId);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<ResponseDto<Supplier>> AddSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(string taxId);
        Task<bool> SupplierExistsAsync(string taxId);

        // Fatura ile ilgili yöntemler
        Task<ResponseDto<Invoice>> GetInvoicesBySupplierIdAsync(string supplierTaxId);
        Task<ResponseDto<Invoice>> RequestEarlyPaymentAsync(string invoiceId);
    }
}
