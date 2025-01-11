namespace InvoiceImportApp.Models;

public class InvoiceHeader
{
    public int InvoiceId { get; set; }      // Primary key for the InvoiceHeader table
    public string InvoiceNumber { get; set; } = ""; // Invoice Number
    public DateTime? InvoiceDate { get; set; } // Nullable DateTime for Invoice Date
    public string? Address { get; set; }      // Address for the Invoice
    public decimal? InvoiceTotal { get; set; } // Nullable float for the total amount

    // Navigation property for related InvoiceLines
    public ICollection<InvoiceLine> InvoiceLines { get; set; }
}
