using FoodDelivery.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.API.Repositories.Interfaces.Sahil
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<OrderItem> CreateAsync(OrderItem orderItem);
        Task<List<OrderItem>> CreateRangeAsync(List<OrderItem> orderItems);
        Task DeleteByOrderIdAsync(int orderId);
    }
}
