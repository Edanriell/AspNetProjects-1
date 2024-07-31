using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
	: IdentityDbContext<IdentityUser>(options)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
	}
}