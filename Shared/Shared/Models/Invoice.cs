namespace Shared.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime TermDate { get; set; }
        public string? BuyerTaxId { get; set; }
        public string? SupplierTaxId { get; set; }
        public decimal InvoiceCost { get; set; }
        public InvoiceStatus Status { get; set; }
    }

    public enum InvoiceStatus
    {
        New = 0,
        Used = 1,
        Paid = 2
    }
}
