using FoodDelivery.API.DTOs.Tushar;

namespace FoodDelivery.API.Services.Interfaces.Tushar
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