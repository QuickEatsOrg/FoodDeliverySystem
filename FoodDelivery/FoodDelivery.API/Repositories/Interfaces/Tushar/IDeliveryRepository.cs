using FoodDelivery.API.Models;

namespace FoodDelivery.API.Repositories.Interfaces.Tushar
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<Order>> GetAllDeliveriesAsync();
        Task<Order?> GetDeliveryByOrderIdAsync(int orderId);
        Task<IEnumerable<Order>> GetDeliveriesByDriverIdAsync(int driverId);
        Task<Order?> AssignDriverAsync(int orderId, int driverId);
        Task<Order?> UpdateDeliveryStatusAsync(int orderId, string orderStatus);
    }
}