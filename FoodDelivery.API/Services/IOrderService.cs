using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, int customerId, string cartId);
        Task<OrderDto> GetOrderByIdAsync(int orderId, int userId, string role);
        Task<List<OrderDto>> GetMyOrdersAsync(int customerId);
        Task<List<OrderDto>> GetRestaurantOrdersAsync(int restaurantId);
        Task<List<OrderDto>> GetDriverOrdersAsync(int driverId);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<bool> CancelOrderAsync(int orderId, int customerId);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto, int userId, string role);
    }
}
