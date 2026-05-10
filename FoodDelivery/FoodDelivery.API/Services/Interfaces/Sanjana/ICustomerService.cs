using FoodDelivery.API.DTOs.Sanjana;

namespace FoodDelivery.API.Services.Interfaces.Sanjana
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
