using InvoiceAppV1;
using InvoiceAppV1.Controllers;
using InvoiceAppV1.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTests;

[Collection("TransactionalTests")]
public class TransactionalInvoiceControllerTests(TransactionalTestDatabaseFixture fixture) : IDisposable
{
    public void Dispose()
    {
        fixture.Cleanup();
    }

    [Fact]
    public async Task UpdateInvoiceStatusAsync_ShouldUpdateStatus()
    {
        // Arrange
        await using var dbContext = fixture.CreateDbContext();
        var emailServiceMock = new Mock<IEmailService>();
        var controller = new InvoiceController(dbContext, emailServiceMock.Object);
        // Act
        var invoice = await dbContext.Invoices.FirstAsync(x => x.Status == InvoiceStatus.AwaitPayment);
        await controller.UpdateInvoiceStatusAsync(invoice.Id, InvoiceStatus.Paid);
        // Assert
        dbContext.ChangeTracker.Clear();
        var updatedInvoice = await dbContext.Invoices.FindAsync(invoice.Id);
        Assert.NotNull(updatedInvoice);
        Assert.Equal(InvoiceStatus.Paid, updatedInvoice.Status);
    }

    // In the preceding code, we use the TransactionalTestDatabaseFixture class to create the
    // database context. This class implements the IDisposable interface and calls the Cleanup()
    // method in the Dispose() method. If we have multiple test methods in one test class, xUnit will
    // create a new instance of the test class for each test method and run them in sequence. Therefore, the
    // Dispose() method will be called after each test method is executed to clean up the database, which
    // will ensure that the changes made in the test methods will not affect other test methods.
    // What if we want to share TransactionalTestDatabaseFixture in multiple test classes?
    // By default, xUnit will run the test classes in parallel. If other test classes also need to use this fixture
    // to clean up the database, it may cause a concurrency issue when xUnit initializes the test context. To
    // avoid this problem, we can use the Collection attribute to specify that the test classes that use this
    // fixture belong to the same test collection so that xUnit will not run them in parallel. 
}