using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);

        Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync();

        Task<CustomerResponseDto> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);

        Task<CustomerResponseDto> GetCustomerByIdAsync(int id);

        Task<CustomerResponseDto> GetCustomerByEmailAync(string email);

        Task<CustomerResponseDto> GetCustomerByPhoneAsync(string phone);
    }
}
