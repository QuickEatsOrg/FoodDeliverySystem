using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services
{
    public interface IDeliveryService
    {
        Task<IEnumerable<DeliveryDto>> GetAllDeliveriesAsync();
        Task<DeliveryDto> GetDeliveryByOrderIdAsync(int orderId);
        Task<IEnumerable<DeliveryDto>> GetDeliveriesByDriverIdAsync(int driverId);
        Task<DeliveryDto> AssignDriverAsync(AssignDriverDto dto);
        Task<DeliveryDto> UpdateDeliveryStatusAsync(int orderId, UpdateDeliveryStatusDto dto);
    }
}