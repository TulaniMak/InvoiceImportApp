using InvoiceImportApp.Data;
using InvoiceImportApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace InvoiceImportApp.Services;

public class InvoiceService
{
    public static void ReadAndStoreCSV(string filePath)
    {
        try
        {
            // Initialize collections for invoice headers and lines
            var invoiceHeaders = new List<InvoiceHeader>();
            var invoiceLines = new List<InvoiceLine>();

            // Open the file for reading
            using (var reader = new StreamReader(filePath))
            {
                // Read the header line (optional, skip if not needed)
                string header = reader.ReadLine();

                // Read the remaining lines (data rows)
                while (!reader.EndOfStream)
                {
                    // Read one line of data
                    string line = reader.ReadLine();

                    // Split the line by commas (CSV delimiter)
                    string[] values = line.Split(',');

                    // Parse the data into InvoiceHeader and InvoiceLine
                    string invoiceNumber = values[0];
                    DateTime invoiceDate = DateTime.ParseExact(values[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    string address = values[2];
                    decimal totalExVAT = decimal.Parse(values[3], CultureInfo.InvariantCulture);
                    string description = values[4];
                    int quantity = int.Parse(values[5]);
                    decimal unitSellingPriceExVAT = decimal.Parse(values[6], CultureInfo.InvariantCulture);

                    // Check if the invoice already exists in the headers collection
                    var existingHeader = invoiceHeaders.FirstOrDefault(h => h.InvoiceNumber == invoiceNumber);
                                       
                    if (existingHeader == null)
                    {
                        // Create a new InvoiceHeader if it doesn't exist
                        var newHeader = new InvoiceHeader
                        {
                            InvoiceNumber = invoiceNumber,
                            InvoiceDate = invoiceDate,
                            Address = address,
                            InvoiceTotal = totalExVAT,
                            InvoiceLines = new List<InvoiceLine>() // Initialize the list for navigation property
                        };

                        invoiceHeaders.Add(newHeader);
                        existingHeader = newHeader;
                    }

                    // Create a new InvoiceLine and associate it with the existing header
                    var newLine = new InvoiceLine
                    {
                        InvoiceNumber = invoiceNumber,
                        Description = description,
                        Quantity = quantity,
                        UnitSellingPriceExVAT = unitSellingPriceExVAT,
                        InvoiceHeader = existingHeader // Associate with the header
                    };

                    invoiceLines.Add(newLine);
                    existingHeader.InvoiceLines.Add(newLine);
                }
            }

            // Store the data in the database
            SaveDataToDatabase(invoiceHeaders, invoiceLines);   
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
        }
    }

    private static void SaveDataToDatabase(List<InvoiceHeader> invoiceHeaders, List<InvoiceLine> invoiceLines)
    {
        try
        {
            using (var context = new ApplicationDbContext())
            {
                // Prevent duplicate inserts for headers
                foreach (var header in invoiceHeaders)
                {
                    if (!context.InvoiceHeaders.Any(h => h.InvoiceNumber == header.InvoiceNumber))
                    {
                        context.InvoiceHeaders.Add(header);
                    }
                }
                // Add InvoiceLines
                context.InvoiceLines.AddRange(invoiceLines);
                context.SaveChanges();
            }
            Console.WriteLine("Data successfully saved to the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving data to the database: {ex.Message}");
        }
    }

    public static void PrintTotalQuantityPerInvoice() {
        using (var context = new ApplicationDbContext())
        {
            var invoiceSummaries = context.InvoiceLines
                .GroupBy(line => line.InvoiceNumber)  // Group by InvoiceNumber
                .Select(group => new
                {
                    InvoiceNumber = group.Key,
                    TotalQuantity = group
                    .Sum(line => line.Quantity)  // Sum of Quantity for each InvoiceNumber
                })
                .ToList();  // Execute the query and get the results

            // Output the results
            foreach (var summary in invoiceSummaries)
            {
                Console.WriteLine($"Invoice Number: {summary.InvoiceNumber}, Total Quantity: {summary.TotalQuantity}");
            }
        }
    }
    
    public static decimal SumHeaderTotalPrice()
    {
        using (var context = new ApplicationDbContext())
        {
            var totalAmount = context.InvoiceHeaders
            .Sum(header => Convert.ToDecimal(header.InvoiceTotal));

             return totalAmount;
        }
    }
    
    public static decimal SumLinesTotalPrice()
    {
         using (var context = new ApplicationDbContext())
         {
            var totalAmount = context.InvoiceLines
            .Sum(line => Convert.ToDecimal(line.UnitSellingPriceExVAT) * Convert.ToDecimal(line.Quantity));

            return totalAmount;
         } 
    }
}
