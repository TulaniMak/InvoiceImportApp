**Description**

This project is a C# Console Application that imports invoice data from a CSV file into a Microsoft SQL Server database. 
It uses Entity Framework Core for database operations and performs validations, calculations, and comparisons to ensure data integrity.

**Features**

- Reads data from a CSV file containing invoice headers and lines.
- Saves data into SQL Server tables (InvoiceHeader and InvoiceLines) using Entity Framework Core.
- Prevents duplicate entries for invoice headers.
- Calculates and compares totals:
- Total price from InvoiceHeader (InvoiceTotal).
- Total price derived from InvoiceLines (Quantity * UnitSellingPriceExVAT).
- Provides logging and error handling for file reading, database operations, and data validation.
- Outputs results and comparisons to the console.

  **Prerequisite**
  
To run the application, ensure the following are installed on your system:
- .NET 6.0 or later
- Microsoft SQL Server
- Visual Studio or any preferred C# IDE
- NuGet Packages:
  - CsvHelper Version="33.0.1" 
	- Microsoft.EntityFrameworkCore" Version="9.0.0"
	- Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" 
	- Microsoft.Extensions.Configuration.Json" Version="9.0.0"
 
**Datasebase Setup**

Run the query
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InvoiceHeader](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [varchar](50) NOT NULL,
	[InvoiceDate] [date] NULL,
	[Address] [varchar](50) NULL,
	[InvoiceTotal] [float] NULL,
 CONSTRAINT [PK_InvoiceHeader] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InvoiceLines](
	[LineId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [varchar](50) NOT NULL,
	[Description] [varchar](100) NULL,
	[Quantity] [float] NULL,
	[UnitSellingPriceExVAT] [float] NULL,
 CONSTRAINT [PK_InvoiceLines] PRIMARY KEY CLUSTERED 
(
	[LineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

 
