using FoodDelivery.API.DTOs.Sanjana;

namespace FoodDelivery.API.Services.Interfaces.Sanjana;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> RegisterCustomerAsync(CreateCustomerDto customerDto);
    Task<bool> RegisterDriverAsync(RegisterDriverDto driverDto);
    Task<bool> RegisterRestaurantAsync(RegisterRestaurantDto restaurantDto);
}
