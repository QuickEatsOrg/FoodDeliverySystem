using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto?>> LoginAsync(LoginDto loginDto);
    Task<bool> RegisterCustomerAsync(CreateCustomerDto customerDto);
    Task<bool> RegisterDriverAsync(RegisterDriverDto driverDto);
    Task<bool> RegisterRestaurantAsync(RegisterRestaurantDto restaurantDto);
}
