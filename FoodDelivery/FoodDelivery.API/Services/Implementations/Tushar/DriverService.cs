using FoodDelivery.API.DTOs.Tushar;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Interfaces.Tushar;
using FoodDelivery.API.Services.Interfaces.Tushar;

namespace FoodDelivery.API.Services.Implementations.Tushar
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<IEnumerable<DriverDto>> GetAllDriversAsync()
        {
            var drivers = await _driverRepository.GetAllDriversAsync();

            return drivers.Select(MapToDriverDto);
        }

        public async Task<DriverDto> GetDriverByIdAsync(int driverId)
        {
            var driver = await _driverRepository.GetDriverByIdAsync(driverId);

            if (driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            return MapToDriverDto(driver);
        }

        public async Task<DriverDto> CreateDriverAsync(CreateDriverDto dto)
        {
            var driver = new DeliveryDriver
            {
                DriverName = dto.DriverName,
                DriverPhone = dto.DriverPhone,
                DriverVehicle = dto.DriverVehicle,
                DriverEmail = dto.DriverEmail,
                RoleId = 4
            };

            var createdDriver = await _driverRepository.CreateDriverAsync(driver);

            return MapToDriverDto(createdDriver);
        }

        public async Task<DriverDto> UpdateDriverAsync(int driverId, UpdateDriverDto dto)
        {
            var driver = new DeliveryDriver
            {
                DriverId = driverId,
                DriverName = dto.DriverName,
                DriverPhone = dto.DriverPhone,
                DriverVehicle = dto.DriverVehicle,
                DriverEmail = dto.DriverEmail,
                RoleId = 4
            };

            var updatedDriver = await _driverRepository.UpdateDriverAsync(driver);

            if (updatedDriver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            return MapToDriverDto(updatedDriver);
        }

        public async Task<bool> DeleteDriverAsync(int driverId)
        {
            var isDeleted = await _driverRepository.DeleteDriverAsync(driverId);

            if (!isDeleted)
            {
                throw new NotFoundException("Driver not found");
            }

            return true;
        }

        private static DriverDto MapToDriverDto(DeliveryDriver driver)
        {
            return new DriverDto
            {
                DriverId = driver.DriverId,
                DriverName = driver.DriverName,
                DriverPhone = driver.DriverPhone,
                DriverVehicle = driver.DriverVehicle,
                DriverEmail = driver.DriverEmail,
                RoleId = driver.RoleId
            };
        }
    }
}