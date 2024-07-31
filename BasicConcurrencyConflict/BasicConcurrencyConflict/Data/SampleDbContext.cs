using BasicConcurrencyConflict.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicConcurrencyConflict.Data;

public class SampleDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public SampleDbContext(DbContextOptions<SampleDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // If we don't use [Timestamp] attribute
        // In the Fluent API configuration, we need to add the following code to map the RowVersion property
        // to the rowversion column in the database
        // modelBuilder.Entity<Product>()
        //     .Property(p => p.RowVersion)
        //     .IsRowVersion();

        //  Configuration for application-managed concurrency token
        // modelBuilder.Entity<Product>()
        //     .Property(p => p.Version)
        //     .IsConcurrencyToken();

        modelBuilder.SeedProductData();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }
}