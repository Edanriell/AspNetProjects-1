using FluentAssertions;
using InvoiceAppV1;
using InvoiceAppV1.Controllers;
using InvoiceAppV1.Models;
using InvoiceAppV1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTests;

// using TestDatabaseFixture

public class InvoiceControllerTests(TestDatabaseFixture fixture) : IClassFixture<TestDatabaseFixture>
{
    [Fact]
    public async Task GetInvoices_ShouldReturnInvoices()
    {
        // Arrange
        await using var dbContext = fixture.CreateDbContext();
        var emailServiceMock = new Mock<IEmailService>();
        var controller = new InvoiceController(dbContext, emailServiceMock.Object);
        // Act
        var actionResult = await controller.GetInvoicesAsync();
        // Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        var returnResult = Assert.IsAssignableFrom<List<Invoice>>(result.Value);
        //Assert.NotNull(returnResult);
        //Assert.Equal(2, returnResult.Count);
        //Assert.Contains(returnResult, i => i.InvoiceNumber == "INV-001");
        //Assert.Contains(returnResult, i => i.InvoiceNumber == "INV-002");
        returnResult.Should().NotBeNull();
        //returnResult.Should().HaveCount(2);
        returnResult.Count.Should().Be(2, "The number of invoices should be 2");
        returnResult.Should().Contain(i => i.InvoiceNumber == "INV-001");
        returnResult.Should().Contain(i => i.InvoiceNumber == "INV-002");

        // In the GetInvoices_ShouldReturnInvoices() method, we use the fixture to create the
        // InvoiceDbContext object, and then create the InvoiceController object with some
        // mocked dependencies. Then, we call the GetInvoicesAsync() method to get the invoices from
        // the database. Finally, we use the Assert class to verify the result.
        // The data we use to verify the controller is the data we seed into the database in the
        // TestDatabaseFixture class. If you change the data in the TestDatabaseFixture class,
        // you also need to change the expected data in the test class.
    }

    [Theory]
    [InlineData(InvoiceStatus.AwaitPayment)]
    [InlineData(InvoiceStatus.Draft)]
    public async Task GetInvoicesByStatus_ShouldReturnInvoices(InvoiceStatus status)
    {
        // Arrange
        await using var dbContext = fixture.CreateDbContext();
        var emailServiceMock = new Mock<IEmailService>();
        var controller = new InvoiceController(dbContext, emailServiceMock.Object);
        // Act
        var actionResult = await controller.GetInvoicesAsync(status: status);
        // Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        var returnResult = Assert.IsAssignableFrom<List<Invoice>>(result.Value);
        Assert.NotNull(returnResult);
        Assert.Single(returnResult);
        Assert.Equal(status, returnResult.First().Status);

        // In the preceding example, we use the Theory attribute to indicate that the test method is a Theory test
        // method. A Theory test method can have one or more InlineData attributes. Each InlineData
        // attribute can pass one value or multiple values to the test method. In this case, we use the InlineData
        // attribute to pass the InvoiceStatus value to the test method. You can use multiple InlineData
        // attributes to pass multiple values to the test method. The test method will be executed multiple times
        // with different values.
    }

    [Fact]
    public async Task CreateInvoice_ShouldCreateInvoice()
    {
        // If a method changes the database, we need to ensure that the database is in a known state before we
        // run the test, and also ensure that the database is restored to its original state so that the change will
        // not affect other tests.

        // Arrange
        await using var dbContext = fixture.CreateDbContext();
        var emailServiceMock = new Mock<IEmailService>();
        var controller = new InvoiceController(dbContext, emailServiceMock.Object);
        // Act
        var contactId = dbContext.Contacts.First().Id;
        var invoice = new Invoice
        {
            DueDate = DateTimeOffset.Now.AddDays(30),
            ContactId = contactId,
            Status = InvoiceStatus.Draft,
            InvoiceDate = DateTimeOffset.Now,
            InvoiceItems = new List<InvoiceItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Description = "Item 1",
                    Quantity = 1,
                    UnitPrice = 100
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Description = "Item 2",
                    Quantity = 2,
                    UnitPrice = 200
                }
            }
        };
        var actionResult = await controller.CreateInvoiceAsync(invoice);
        // Assert
        var result = actionResult.Result as CreatedAtActionResult;
        Assert.NotNull(result);
        var returnResult = Assert.IsAssignableFrom<Invoice>(result.Value);
        var invoiceCreated = await dbContext.Invoices.FindAsync(returnResult.Id);

        Assert.NotNull(invoiceCreated);
        Assert.Equal(InvoiceStatus.Draft, invoiceCreated.Status);
        Assert.Equal(500, invoiceCreated.Amount);
        Assert.Equal(3, dbContext.Invoices.Count());
        Assert.Equal(contactId, invoiceCreated.ContactId);
        // You can add more assertions here

        // Clean up
        dbContext.Invoices.Remove(invoiceCreated);
        await dbContext.SaveChangesAsync();

        // In this test method, we create a new invoice and pass it to the CreateInvoiceAsync()
        // method. Then, we use the Assert class to verify the result. Finally, we remove the invoice from the
        // database and save the changes. Note that the result of the CreateInvoiceAsync() method is
        // a CreatedActionResult object, which contains the created invoice. So, we should convert the
        // result into a CreatedAtActionResult object, and then get the created invoice from the Value
        // property. Also, in this test method, we have asserted that the Amount property of the created invoice
        // is correct based on the invoice items.
        // In the preceding example, the data was created in the test method and then removed from the
        // database after the test. There is another way to manage this scenario: using a transaction. We can use
        // a transaction to wrap the test method, and then roll back the transaction after the test. So, the data
        // created in the test method will not be saved to the database. In this way, we do not need to manually
        // remove the data from the database.
    }

    [Fact]
    public async Task UpdateInvoice_ShouldUpdateInvoice()
    {
        // Arrange
        await using var dbContext = fixture.CreateDbContext();
        var emailServiceMock = new Mock<IEmailService>();
        var controller = new InvoiceController(dbContext, emailServiceMock.Object);
        // Act
        // Start a transaction to prevent the changes from being saved to the database
        await dbContext.Database.BeginTransactionAsync();
        var invoice = dbContext.Invoices.First();
        invoice.Status = InvoiceStatus.Paid;
        invoice.Description = "Updated description";
        invoice.InvoiceItems.ForEach(x =>
        {
            x.Description = "Updated description";
            x.UnitPrice += 100;
        });
        var expectedAmount = invoice.InvoiceItems.Sum(x => x.UnitPrice * x.Quantity);
        await controller.UpdateInvoiceAsync(invoice.Id, invoice);

        // Assert
        dbContext.ChangeTracker.Clear();
        var invoiceUpdated = await dbContext.Invoices.SingleAsync(x => x.Id == invoice.Id);
        //Assert.Equal(InvoiceStatus.Paid, invoiceUpdated.Status);
        //Assert.Equal("Updated description", invoiceUpdated.Description);
        //Assert.Equal(expectedAmount, invoiceUpdated.Amount);
        //Assert.Equal(2, dbContext.Invoices.Count());
        invoiceUpdated.Status.Should().Be(InvoiceStatus.Paid);
        invoiceUpdated.Description.Should().Be("Updated description");
        invoiceUpdated.Amount.Should().Be(expectedAmount);
        invoiceUpdated.InvoiceItems.Should().HaveCount(2);

        // In the UpdateInvoice_ShouldUpdateInvoice() method, before we call the
        // UpdateInvoiceAsync() method, we start a transaction. After the test method is executed, we
        // do not commit the transaction, so the transaction will roll back. The changes that are made in the test
        // method will not be saved to the database. In this way, we do not need to manually remove the data
        // from the database.
        // We also use the ChangeTracker.Clear() method to clear the change tracker. The change tracker
        // is used to track the changes made to the entities. If we do not clear the change tracker, we will get the
        // tracked entities instead of querying the database. So, we need to explicitly clear the change tracker
        // before we query the database.
        // This approach is convenient when we test the methods that change the database. However, it can lead
        // to a problem: what if the controller (or the service) method already starts a transaction? We cannot
        // wrap a transaction in another transaction. In this case, we must explicitly clean up any changes made
        // to the database after the test method is executed.
    }
}