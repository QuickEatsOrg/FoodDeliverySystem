using FoodDelivery.API.DTOs.Tushar;

namespace FoodDelivery.API.Services.Interfaces.Tushar
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