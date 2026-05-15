using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services
{
    public interface IDriverAuthService
    {
        Task<DriverLoginResponseDto> LoginAsync(DriverLoginDto dto);
    }
}