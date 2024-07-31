using IntegrationTests.Helpers;
using InvoiceAppV3.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public class CustomIntegrationTestsFixture : WebApplicationFactory<Program>
{
    private const string ConnectionString =
        @"Server=(localdb)\mssqllocaldb;Database=InvoiceIntegrationTestDb;Trusted_Connection=True";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set up a test database
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<InvoiceDbContext>));
            services.Remove(descriptor);
            services.AddDbContext<InvoiceDbContext>(options => { options.UseSqlServer(ConnectionString); });
            using var scope = services.BuildServiceProvider().CreateScope();
            var scopeServices = scope.ServiceProvider;
            var dbContext = scopeServices.GetRequiredService<InvoiceDbContext>();
            Utilities.InitializeDatabase(dbContext);
        });

        //builder.ConfigureAppConfiguration((context, config) =>
        //{
        //    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        //});
        //builder.UseEnvironment("Development");
    }
    
    // In the preceding code, we override the ConfigureWebHost() method to configure the test
    // web host for the SUT. When the test web host is created, the Program class will execute first,
    // which means the default database context defined in the Program class will be created. Then the
    // ConfigureWebHost() method defined in the CustomIntegrationTestsFixture class will
    // be executed. So we need to find the default database context using services.SingleOrDefault(d
    // => d.ServiceType == typeof(DbContextOptions<InvoiceDbContext>)) and
    // then remove it from the service collection. Then we add a new database context that uses the test
    // database. This approach allows us to use a separate database for integration tests. We also need to
    // create the test database and seed some test data when we initialize the test fixture.
}