using InvoiceAppV3.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAppV3.Data;

public class InvoiceDbContext(DbContextOptions<InvoiceDbContext> options, IConfiguration? configuration)
    : DbContext(options)
{
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(configuration?.GetConnectionString("DefaultConnection"));
    }
}