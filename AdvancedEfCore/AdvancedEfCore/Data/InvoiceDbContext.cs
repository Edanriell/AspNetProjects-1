using AdvancedEfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedEfCore.Data;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-001",
                ContactName = "Iron Man",
                Description = "Invoice for the first month",
                Amount = 100,
                InvoiceDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                DueDate = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero),
                Status = InvoiceStatus.AwaitPayment
            },
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-002",
                ContactName = "Captain America",

                Description = "Invoice for the first month",
                Amount = 200,
                InvoiceDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                DueDate = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero),
                Status = InvoiceStatus.AwaitPayment
            },
            new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-003",
                ContactName = "Thor",
                Description = "Invoice for the first month",
                Amount = 300,
                InvoiceDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                DueDate = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero),
                Status = InvoiceStatus.Draft
            });

        // Use extension methods to configure the model
        //modelBuilder.ConfigureInvoice();

        // Use IEntityTypeConfiguration<TEntity> interface
        //modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        // Or
        //new InvoiceConfiguration().Configure(modelBuilder.Entity<Invoice>());

        // Grouping the configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceDbContext).Assembly);
    }

    // This enables no tracking globally but if we want it to remove locally we can use .AsNoTracking()
    // no tracking is used to read only queries to improve performance
    // if we want tracking we use AsTracking()
    // But AsTracking is enabled by default
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    // If an entity is a keyless entity, EF Core will never track it. Keyless entity types do not
    // have keys defined on them. They are configured by a [Keyless] data annotation or a
    // Fluent API HasNoKey() method. The keyless entity is often used for read-only queries
    // or views. 
}