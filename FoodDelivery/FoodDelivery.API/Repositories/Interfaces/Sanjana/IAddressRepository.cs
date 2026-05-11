using FoodDelivery.API.Models;

namespace FoodDelivery.API.Repositories.Interfaces.Sanjana
{
    public interface IAddressRepository
    {
        Task<DeliveryAddress> CreateAddressAsync(DeliveryAddress address);
        Task<DeliveryAddress> UpdateAddressAsync(DeliveryAddress address);
        Task<List<DeliveryAddress>> GetAllAddressesByCustomerIdAsync(int customerId);
        Task<DeliveryAddress?> GetAddressByAddressIdAsync(int addressId);
        Task<int?> GetAddressCountByCustomerIdAsync(int customerId);
    }
}