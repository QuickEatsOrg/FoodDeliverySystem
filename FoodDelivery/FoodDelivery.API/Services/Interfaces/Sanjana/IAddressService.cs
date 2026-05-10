using FoodDelivery.API.DTOs.Sanjana;

namespace FoodDelivery.API.Services.Interfaces.Sanjana
{
    public interface IAddressService
    {
        Task<AddressResponseDto> CreateAddressAsync(int customerId, CreateAddressDto dto);
        Task<AddressResponseDto> UpdateAddressAsync(int addressId, UpdateAddressDto dto);
        Task<AddressResponseDto> GetAddressByAddressIdAsync(int addressId);
        Task<List<AddressResponseDto>> GetAllAddressesByCustomerIdAsync(int customerId);
    }
}