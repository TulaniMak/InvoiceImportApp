using InvoiceImportApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceImportApp.Data;

public class ApplicationDbContext : DbContext
{

    public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    // Configure entity relationships, constraints, and database specifics
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the primary key for InvoiceHeaders
        modelBuilder.Entity<InvoiceHeader>()
        .HasKey(h => h.InvoiceNumber);

        // Configure the primary key for InvoiceLine
        modelBuilder.Entity<InvoiceLine>()
            .HasKey(i => i.LineId);  // LineId as the primary key for InvoiceLine

        modelBuilder.Entity<InvoiceHeader>()
        .Property(i => i.InvoiceId)
        .ValueGeneratedOnAdd(); // Make sure it's auto-generated

        // Configure the relationship between InvoiceHeader and InvoiceLine
        modelBuilder.Entity<InvoiceLine>()
            .HasOne(i => i.InvoiceHeader)
            .WithMany(h => h.InvoiceLines)
            .HasForeignKey(i => i.InvoiceNumber);  // Foreign key in InvoiceLine

        // Additional configuration can go here if needed, like table names or constraints
        modelBuilder.Entity<InvoiceHeader>().ToTable("InvoiceHeader");
        modelBuilder.Entity<InvoiceLine>().ToTable("InvoiceLines");

        
    }
}
