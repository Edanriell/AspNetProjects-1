using InvoiceAppV1;
using InvoiceAppV1.Data;
using InvoiceAppV1.Models;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

// When we test the CRUD methods against the database, we need to prepare the database before the
// tests are executed, and then clean up the database after the tests are completed so that the changes
// made by the tests will not affect other tests. xUnit provides the IClassFixture<T> interface to
// create a test fixture, which can be used to prepare and clean up the database for each test class.

public class TestDatabaseFixture
{
    // In the TestDatabaseFixture class, we define a connection string to the local database. Using
    // a const string is for simplicity only. In a real application, you may want to use the configuration
    // system to read the connection string from other sources, such as the appsettings.json file.
    private const string ConnectionString =
        @"Server=(localdb)\mssqllocaldb;Database=InvoiceTestDb;Trusted_Connection=True";

    private static readonly object Lock = new();
    private static bool _databaseInitialized;

    // We need to call the InitializeDatabase() method to initialize the database in the
    // constructor of the TestDatabaseFixture class
    public TestDatabaseFixture()
    {
        // This code comes from Microsoft Docs: https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Testing/TestingWithTheDatabase/TestDatabaseFixture.cs
        lock (Lock)
        {
            if (!_databaseInitialized!)
            {
                InitializeDatabase();
                _databaseInitialized = true;
            }
        }
        // To avoid initializing the database multiple times, we use a static field, _databaseInitialized,
        // to indicate whether the database has been initialized. We also define a static object, Lock, to ensure
        // that the database is initialized only once. The InitializeDatabase() method is used to initialize
        // the database. It will only be called once before the tests are executed
    }

    // Then, we add a method to create the database context object
    public InvoiceDbContext CreateDbContext()
    {
        return new InvoiceDbContext(new DbContextOptionsBuilder<InvoiceDbContext>()
            .UseSqlServer(ConnectionString)
            .Options, null);
    }

    // We also need a method to initialize the database
    // In the InitializeDatabase() method, we create a new InvoiceDbContext object and
    // then use the EnsureDeleted() method to delete the database if it exists. Then, we use the
    // EnsureCreated() method to create the database. After that, we seed some data into the database. 
    public void InitializeDatabase()
    {
        using var context = CreateDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Create a few Contacts
        var contacts = new List<Contact>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com"
            }
        };
        context.Contacts.AddRange(contacts);
        // Create a few Invoices
        var invoices = new List<Invoice>
        {
            new()
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-001",
                Amount = 500,
                DueDate = DateTimeOffset.Now.AddDays(30),
                Contact = contacts[0],
                Status = InvoiceStatus.AwaitPayment,
                InvoiceDate = DateTimeOffset.Now,
                InvoiceItems = new List<InvoiceItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Description = "Item 1",
                        Quantity = 1,
                        UnitPrice = 100,
                        Amount = 100
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Description = "Item 2",
                        Quantity = 2,
                        UnitPrice = 200,
                        Amount = 400
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = "INV-002",
                Amount = 1000,
                DueDate = DateTimeOffset.Now.AddDays(30),
                Contact = contacts[1],
                Status = InvoiceStatus.Draft,
                InvoiceDate = DateTimeOffset.Now,
                InvoiceItems = new List<InvoiceItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Description = "Item 1",
                        Quantity = 2,
                        UnitPrice = 100,
                        Amount = 200
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Description = "Item 2",
                        Quantity = 4,
                        UnitPrice = 200,
                        Amount = 800
                    }
                }
            }
        };
        context.Invoices.AddRange(invoices);
        context.SaveChanges();
    }
}