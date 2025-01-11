
using InvoiceImportApp.Services;

string csvFilePath = @"C:\Users\Tulan\Downloads\data.csv";

// Call the method to read and process the CSV data
Console.WriteLine("------------------------- Store data to the database -------------------------");
InvoiceService.ReadAndStoreCSV(csvFilePath);
Console.WriteLine("---------------------------------------------------------------------------\n");

//Display invoice number and total quantity (sum of InvoiceLines.Quantity) for all the lines on an invoice, to the console for each invoice imported

Console.WriteLine("------------------------- Print total quantity -------------------------\n");
InvoiceService.PrintTotalQuantityPerInvoice();
Console.WriteLine("---------------------------------------------------------------------------\n");

//check that the sum of InvoiceLines.Quantity * InvoiceLines.UnitSellingPriceExVAT balances back to the sum of all InvoiceHeader.InvoiceTotal
var sumHeaderTotalPrice = InvoiceService.SumHeaderTotalPrice();
var sumLinesTotalPrice = InvoiceService.SumLinesTotalPrice();

// Compare and output the result
bool areEqual = sumHeaderTotalPrice == sumLinesTotalPrice;

Console.WriteLine("------------------------- Check if sums are equal -------------------------\n");
Console.WriteLine($"Is Headers Total Price and Lines Total Price equal: {areEqual} " +
    $"\nHeaders Price Sum: {sumHeaderTotalPrice}" +
    $"\nLines Price Sum: {sumLinesTotalPrice}");
Console.WriteLine("---------------------------------------------------------------------------");
