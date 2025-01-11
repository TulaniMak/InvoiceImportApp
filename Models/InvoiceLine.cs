namespace InvoiceImportApp.Models;

public class InvoiceLine
{
    public int LineId { get; set; }            // Primary key for the InvoiceLines table
    public string InvoiceNumber { get; set; } = ""; // Invoice Number, foreign key reference
    public string Description { get; set; } = "";   // Description of the line item
    public double? Quantity { get; set; }      // Nullable Quantity
    public decimal? UnitSellingPriceExVAT { get; set; } // Nullable Unit Price

    // Navigation property to relate to InvoiceHeader (optional)
    public InvoiceHeader InvoiceHeader { get; set; }
}
