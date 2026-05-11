using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Interfaces.Sahil;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.API.Repositories.Implementations.Sahil
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
                .Include(oi => oi.Item)   // loads MenuItem so we can get name & price
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<OrderItem> CreateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<List<OrderItem>> CreateRangeAsync(List<OrderItem> orderItems)
        {
            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();
            return orderItems;
        }

        public async Task DeleteByOrderIdAsync(int orderId)
        {
            var items = await GetByOrderIdAsync(orderId);
            _context.OrderItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
