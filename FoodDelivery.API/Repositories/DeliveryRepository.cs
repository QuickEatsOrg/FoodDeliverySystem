//using FoodDelivery.API.Data;
using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public DeliveryRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllDeliveriesAsync()
        {
            return await _context.Orders
                .Include(o => o.DeliveryDriver)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetDeliveryByOrderIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.DeliveryDriver)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetDeliveriesByDriverIdAsync(int driverId)
        {
            return await _context.Orders
                .Include(o => o.DeliveryDriver)
                .Where(o => o.DeliveryDriverId == driverId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> AssignDriverAsync(int orderId, int driverId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return null;
            }

            var driverExists = await _context.DeliveryDrivers
                .AnyAsync(d => d.DriverId == driverId);

            if (!driverExists)
            {
                return null;
            }

            order.DeliveryDriverId = driverId;
            order.OrderStatus = "Assigned";

            await _context.SaveChangesAsync();

            return await _context.Orders
                .Include(o => o.DeliveryDriver)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order?> UpdateDeliveryStatusAsync(int orderId, string orderStatus)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return null;
            }

            order.OrderStatus = orderStatus;

            await _context.SaveChangesAsync();

            return await _context.Orders
                .Include(o => o.DeliveryDriver)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}