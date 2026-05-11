using FoodDelivery.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.API.Repositories.Interfaces.Sahil
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetByCustomerIdAsync(int customerId);
        Task<List<Order>> GetByRestaurantIdAsync(int restaurantId);
        Task<List<Order>> GetByDriverIdAsync(int driverId);
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetPendingOrdersAsync();
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<Order?> UpdateStatusAsync(int orderId, string status);
        Task<bool> CancelAsync(int orderId);
    }
}
