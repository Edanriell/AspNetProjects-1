using BasicWebApi.Core.Models;

namespace BasicWebApi.Core.Services;

public interface ICustomerService
{
	Task<List<Customer>> GetCustomerAsync();
	Task<Customer?> GetCustomerAsync(int id);
	Task<Customer> CreateCustomerAsync(Customer user);
	Task<Customer> UpdateCustomerAsync(Customer user);
	Task DeleteCustomerAsync(int id);
}