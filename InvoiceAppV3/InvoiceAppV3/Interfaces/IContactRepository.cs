using InvoiceAppV3.Models;

namespace InvoiceAppV3.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetContactAsync(Guid id);
    Task<IEnumerable<Contact>> GetContactsAsync(int page = 1, int pageSize = 10);
    Task<Contact> CreateContactAsync(Contact contact);
    Task<Contact?> UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(Guid id);
}