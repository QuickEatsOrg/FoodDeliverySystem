using FoodDelivery.API.Models;

namespace FoodDelivery.API.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomerAsync(Customer customer);

        Task<Customer?> GetCustomerByIdAsync(int id);

        Task<Customer?> GetCustomerByEmailAsync(string email);

        Task<Customer?> GetCustomerByPhoneAsynnc(string phone);

        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer> UpdateCustomerAsync(Customer customer);

        Task<bool> EmailExistsAsync(string email, int? excludeId = null);

        Task<bool> PhoneNumberExistsAsync(string phone, int? excludeId = null);

        Task<int> GetCustomerOrderCountAsync(int customerId);
    }
}
