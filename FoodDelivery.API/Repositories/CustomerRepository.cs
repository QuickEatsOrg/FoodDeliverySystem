using FoodDelivery.API.Data;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public CustomerRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.Include(c => c.DeliveryAddresses).ToListAsync();
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers.Include(c => c.DeliveryAddresses).FirstOrDefaultAsync(c => c.CustomerEmail == email);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.Include(c => c.DeliveryAddresses).FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<Customer?> GetCustomerByPhoneAsynnc(string phone)
        {
            return await _context.Customers.Include(c => c.DeliveryAddresses).FirstOrDefaultAsync(c => c.CustomerPhone == phone);
        }

        public async Task<int> GetCustomerOrderCountAsync(int customerId) =>
            await _context.Orders.CountAsync(o => o.CustomerId == customerId);

        public async Task<bool> PhoneNumberExistsAsync(string phone, int? excludeId)
        {
            var query = _context.Customers.Where(p => p.CustomerPhone == phone);
            if (excludeId.HasValue)
                query = query.Where(c => c.CustomerId != excludeId.Value);
            return await query.AnyAsync();
        }
        public async Task<bool> EmailExistsAsync(string email, int? excludeId)
        {
            var query = _context.Customers.Where(c => c.CustomerEmail == email);
            if (excludeId != null)
                query = query.Where(c => c.CustomerId != excludeId.Value);
            return await query.AnyAsync();
        }

    }
}
