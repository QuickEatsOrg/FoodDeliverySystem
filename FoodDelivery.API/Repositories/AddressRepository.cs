using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly FoodDeliveryDbContext _context;
        public AddressRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }
        public async Task<DeliveryAddress> CreateAddressAsync(DeliveryAddress address)
        {
            await _context.DeliveryAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<DeliveryAddress?> GetAddressByAddressIdAsync(int addressId)
        {
            return await _context.DeliveryAddresses.FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task<int?> GetAddressCountByCustomerIdAsync(int customerId)
        {
            return await _context.DeliveryAddresses.CountAsync(a => a.CustomerId == customerId);
        }

        public async Task<List<DeliveryAddress>> GetAllAddressesByCustomerIdAsync(int customerId)
        {
            return await _context.DeliveryAddresses.Where(a => a.CustomerId == customerId).ToListAsync();
        }

        public async Task<DeliveryAddress> UpdateAddressAsync(DeliveryAddress address)
        {
            _context.DeliveryAddresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}