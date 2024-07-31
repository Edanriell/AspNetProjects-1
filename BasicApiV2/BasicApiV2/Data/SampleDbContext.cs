using BasicApiV2.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicApiV2.Data;

public class SampleDbContext(DbContextOptions<SampleDbContext> options, IConfiguration configuration)
	: DbContext(options)
{
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Post>     Posts      => Set<Post>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.SeedData();
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
		                            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
	}
}