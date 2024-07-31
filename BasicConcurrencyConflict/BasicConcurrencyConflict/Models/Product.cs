using System.ComponentModel.DataAnnotations;

namespace BasicConcurrencyConflict.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;

    public int Inventory { get; set; }

    // Native database-generated concurrency token
    // Some databases, such as SQL Server, provide a native mechanism to handle concurrency conflicts.
    // To use the native database-generated concurrency token in SQL Server, we need to create a new
    // property for the Product class and add a [Timestamp] attribute to it.
    // public byte[] RowVersion { get; set; }
    // If we use attribute [Timestamp] EF Core will automatically map it to the rowversion column in the database
    // We don't need to d additional configuration
    [Timestamp] public byte[] RowVersion { get; set; }

    // Application-managed concurrency token
    // Add a new property as the concurrency token
    // public Guid Version { get; set; }
    // If we go by this approach additional configuration is needed
    // modelBuilder.Entity<Product>()
    // .Property(p => p.Version)
    //     .IsConcurrencyToken();
    // Or we can do  this, if we do this we dont need additional configuration
    // [ConcurrencyCheck]
    // public Guid Version { get; set; }
    // When we use application-managed concurrency token we need manually update Version
}