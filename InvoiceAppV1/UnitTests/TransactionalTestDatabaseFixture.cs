using InvoiceAppV1;
using InvoiceAppV1.Data;
using InvoiceAppV1.Models;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

// We can use the IDisposable interface to clean up the database in our tests. To do this, we can create a
// test class that implements the IDisposable interface, and then clean up the database in the Dispose()
// method.
public class TransactionalTestDatabaseFixture
{
    private const string ConnectionString =
        @"Server=(localdb)\mssqllocaldb;Database=InvoiceTransactionalDb;Trusted_Connection=True";

    public TransactionalTestDatabaseFixture()
    {
        // This code comes from Microsoft Docs: https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Testing/TestingWithTheDatabase/TransactionalTestDatabaseFixture.cs
        using var context = CreateDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        InitializeDatabase();
    }

    public InvoiceDbContext CreateDbContext()
    {
        return new InvoiceDbContext(new DbContextOptionsBuilder<InvoiceDbContext>()
            .UseSqlServer(ConnectionString)
            .Options, null);
    }

    public void InitializeDatabase()
    {
        using var context = CreateDbContext();
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

    public void Cleanup()
    {
        using var context = CreateDbContext();
        context.Contacts.ExecuteDelete();
        context.Invoices.ExecuteDelete();
        context.SaveChanges();
        InitializeDatabase();
    }

    // In the preceding code, we create a database called InvoiceTransactionalTestDb and initialize
    // it. This file is similar to the InvoiceTestDatabaseFixture class, except that it has a Cleanup
    // method, which is used to clean up the database. In the Cleanup method, we delete all the contacts
    // and invoices from the database and then initialize the database to restore the data.
}