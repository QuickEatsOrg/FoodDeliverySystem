using FoodDelivery.API.Models;

namespace FoodDelivery.API.Repositories.Interfaces.Tushar
{
    public interface IDriverRepository
    {
        Task<IEnumerable<DeliveryDriver>> GetAllDriversAsync();
        Task<DeliveryDriver?> GetDriverByIdAsync(int driverId);
        Task<DeliveryDriver?> GetDriverByEmailAsync(string driverEmail);
        Task<DeliveryDriver> CreateDriverAsync(DeliveryDriver driver);
        Task<DeliveryDriver?> UpdateDriverAsync(DeliveryDriver driver);
        Task<bool> DeleteDriverAsync(int driverId);
    }
}