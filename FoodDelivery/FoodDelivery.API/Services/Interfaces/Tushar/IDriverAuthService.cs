using FoodDelivery.API.DTOs.Tushar;

namespace FoodDelivery.API.Services.Interfaces.Tushar
{
    public interface IDriverAuthService
    {
        Task<DriverLoginResponseDto> LoginAsync(DriverLoginDto dto);
    }
}