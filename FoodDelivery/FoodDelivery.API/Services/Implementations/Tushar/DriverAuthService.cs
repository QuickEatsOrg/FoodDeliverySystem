using FoodDelivery.API.DTOs.Tushar;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Helpers;
using FoodDelivery.API.Repositories.Interfaces.Tushar;
using FoodDelivery.API.Services.Interfaces.Tushar;

namespace FoodDelivery.API.Services.Implementations.Tushar
{
    public class DriverAuthService : IDriverAuthService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly JwtHelper _jwtHelper;

        public DriverAuthService(IDriverRepository driverRepository, JwtHelper jwtHelper)
        {
            _driverRepository = driverRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<DriverLoginResponseDto> LoginAsync(DriverLoginDto dto)
        {
            var driver = await _driverRepository.GetDriverByEmailAsync(dto.DriverEmail);

            if (driver == null)
            {
                throw new BadRequestException("Invalid email or password");
            }

            if (driver.DriverUnhashedPassword != dto.Password)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var token = _jwtHelper.GenerateToken(
                driver.DriverId,
                driver.DriverName,
                driver.DriverEmail,
                "DeliveryDriver"
            );

            return new DriverLoginResponseDto
            {
                DriverId = driver.DriverId,
                DriverName = driver.DriverName,
                DriverEmail = driver.DriverEmail,
                Role = "DeliveryDriver",
                Token = token
            };
        }
    }
}