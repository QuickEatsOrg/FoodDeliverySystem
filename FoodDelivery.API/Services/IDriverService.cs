using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services
{
    public interface IDriverService
    {
        Task<IEnumerable<DriverDto>> GetAllDriversAsync();
        Task<DriverDto> GetDriverByIdAsync(int driverId);
        Task<DriverDto> CreateDriverAsync(CreateDriverDto dto);
        Task<DriverDto> UpdateDriverAsync(int driverId, UpdateDriverDto dto);
        Task<bool> DeleteDriverAsync(int driverId);
    }
}