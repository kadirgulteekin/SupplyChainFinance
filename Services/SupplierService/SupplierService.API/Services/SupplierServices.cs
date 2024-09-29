using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Models;
using SupplierService.Infrastructure.Data;

namespace SupplierService.API.Services
{
    public class SupplierServices : ISupplierServices
    {

        private readonly SupplierDbContext _supplierDbContext;

        public SupplierServices(SupplierDbContext supplierDbContext)
        {

            _supplierDbContext = supplierDbContext;

        }

        public async Task<ResponseDto<Supplier>> AddSupplierAsync(Supplier supplier)
        {
            _supplierDbContext.Suppliers.Add(supplier);
            await _supplierDbContext.SaveChangesAsync();

            return ResponseDto<Supplier>.Success(supplier, StatusCodes.Status201Created);

        }

        public async Task DeleteSupplierAsync(string taxId)
        {
            var supplier = await _supplierDbContext.Suppliers.FindAsync(taxId);
            if (supplier != null)
            {
                _supplierDbContext.Suppliers.Remove(supplier);
                await _supplierDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _supplierDbContext.Suppliers.ToListAsync();
        }


        public async Task<ResponseDto<Invoice>> GetInvoicesBySupplierIdAsync(string supplierTaxId)
        {
            var invoice = await _supplierDbContext.Invoices.FindAsync(supplierTaxId);

            return ResponseDto<Invoice>.Success(invoice, StatusCodes.Status200OK);
        }

        public async Task<Supplier> GetSupplierByTaxIdAsync(string taxId)
        {
            var suplier = await _supplierDbContext.Suppliers
                .FirstOrDefaultAsync(x => x.TaxId == taxId);
            return suplier;
        }


        public async Task<ResponseDto<Invoice>> RequestEarlyPaymentAsync(string invoiceId)
        {
            var invoice = await _supplierDbContext.Invoices.FindAsync(invoiceId);
            if (invoice == null)
            {
                return ResponseDto<Invoice>.Failed("Invoice not found.", StatusCodes.Status404NotFound);
            }

            invoice.Status = InvoiceStatus.Used;
            await _supplierDbContext.SaveChangesAsync();

            return ResponseDto<Invoice>.Success(invoice, StatusCodes.Status200OK);
        }

        public async Task<bool> SupplierExistsAsync(string taxId)
        {
            return await _supplierDbContext.Suppliers.AnyAsync(s => s.TaxId == taxId);
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _supplierDbContext.Suppliers.Update(supplier);
            await _supplierDbContext.SaveChangesAsync();
        }
    }
}
