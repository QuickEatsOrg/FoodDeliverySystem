using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.API.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public OrderItemRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Item)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<OrderItem> orderItems)
        {
            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();
        }
    }
}
