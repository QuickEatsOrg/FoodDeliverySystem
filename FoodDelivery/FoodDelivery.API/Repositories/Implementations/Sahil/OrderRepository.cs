using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Interfaces.Sahil;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.API.Repositories.Implementations.Sahil
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public OrderRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
                .Include(o => o.Restaurant)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
                .Include(o => o.Restaurant)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByDriverIdAsync(int driverId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
                .Include(o => o.Restaurant)
                .Include(o => o.Customer)
                .Where(o => o.DeliveryDriverId == driverId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
                .Include(o => o.Restaurant)
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderStatus == "Pending")
                .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateStatusAsync(int orderId, string status)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null)
            {
                order.OrderStatus = status;
                await _context.SaveChangesAsync();
            }
            return order;
        }

        public async Task<bool> CancelAsync(int orderId)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null && order.OrderStatus == "Pending")
            {
                order.OrderStatus = "Cancelled";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
