using FoodDelivery.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.API.Repositories
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetByOrderIdAsync(int orderId);
        Task CreateRangeAsync(IEnumerable<OrderItem> items);
    }
}
