using InvoiceAppV1.Controllers;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

[Collection("TransactionalTests")]
public class TransactionalContactControllerTests(TransactionalTestDatabaseFixture fixture) : IDisposable
{
    // In this class, we declare that the TransactionalTestDatabaseFixture class is a collection
    // fixture that uses the CollectionDefinition attribute. We also specify a name for this collection,
    // which is TransactionalTests. Then, we use the ICollectionFixture<T> interface to
    // specify that the TransactionalTestDatabaseFixture class is a collection fixture.
    // After that, we add the Collection attribute to the test classes, which specifies that the TransactionalInvoiceControllerTests and TransactionalContactControllerTests
    // classes belong to the TransactionalTests collection

    public void Dispose()
    {
        fixture.Cleanup();
    }

    [Fact]
    public async Task UpdateContactAsync_ShouldUpdateContact()
    {
        // Arrange
        await using var dbContext = fixture.CreateDbContext();
        var controller = new ContactController(dbContext);
        // Act
        var contact = await dbContext.Contacts.FirstAsync(x => x.FirstName == "John");
        contact.FirstName = "Johnathan";
        await controller.UpdateContactAsync(contact.Id, contact);
        // Assert
        dbContext.ChangeTracker.Clear();
        var updatedContact = await dbContext.Contacts.FindAsync(contact.Id);
        Assert.NotNull(updatedContact);
        Assert.Equal("Johnathan", updatedContact.FirstName);
    }

    // Now, if we debug the tests, we will find that the constructor of the TransactionalTestDatabaseFixture class is only called once, which means that xUnit will only create one instance of
    // the TransactionalTestDatabaseFixture class for these two test classes. Also, xUnit will
    // not run these two test classes in parallel, which means that the Cleanup method of the TransactionalTestDatabaseFixture class will not be called at the same time. So, we can use the
    // TransactionalTestDatabaseFixture class to clean up the database for each test method
    // in multiple test classes.
}
