//using FoodDelivery.API.Data;
using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Interfaces.Tushar;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories.Implementations.Tushar
{
    public class DriverRepository : IDriverRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public DriverRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryDriver>> GetAllDriversAsync()
        {
            return await _context.DeliveryDrivers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DeliveryDriver?> GetDriverByIdAsync(int driverId)
        {
            return await _context.DeliveryDrivers
                .FirstOrDefaultAsync(d => d.DriverId == driverId);
        }

        public async Task<DeliveryDriver?> GetDriverByEmailAsync(string driverEmail)
        {
            return await _context.DeliveryDrivers
                .FirstOrDefaultAsync(d => d.DriverEmail == driverEmail);
        }

        public async Task<DeliveryDriver> CreateDriverAsync(DeliveryDriver driver)
        {
            await _context.DeliveryDrivers.AddAsync(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        public async Task<DeliveryDriver?> UpdateDriverAsync(DeliveryDriver driver)
        {
            var existingDriver = await _context.DeliveryDrivers
                .FirstOrDefaultAsync(d => d.DriverId == driver.DriverId);

            if (existingDriver == null)
            {
                return null;
            }

            existingDriver.DriverName = driver.DriverName;
            existingDriver.DriverPhone = driver.DriverPhone;
            existingDriver.DriverVehicle = driver.DriverVehicle;
            existingDriver.DriverEmail = driver.DriverEmail;

            await _context.SaveChangesAsync();

            return existingDriver;
        }

        public async Task<bool> DeleteDriverAsync(int driverId)
        {
            var driver = await _context.DeliveryDrivers
                .FirstOrDefaultAsync(d => d.DriverId == driverId);

            if (driver == null)
            {
                return false;
            }

            _context.DeliveryDrivers.Remove(driver);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}