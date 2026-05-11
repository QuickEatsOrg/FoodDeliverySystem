using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services;

public interface IRestaurantService
{
    Task<IEnumerable<RestaurantDto>> GetAllAsync();
    Task<RestaurantDto> GetByIdAsync(int id);
    Task<RestaurantDto> CreateAsync(CreateRestaurantDto restaurantDto);
    Task<RestaurantDto> UpdateAsync(int id, UpdateRestaurantDto restaurantDto);
    Task<IEnumerable<MenuItemDto>> GetByRestaurantIdAsync(int restaurantId);
}