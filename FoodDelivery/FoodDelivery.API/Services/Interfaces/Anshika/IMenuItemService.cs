using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Services;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItemDto>> GetAllAsync();
    Task<MenuItemDto> GetByIdAsync(int id);
    Task<MenuItemDto> CreateAsync(CreateMenuItemDto menuItemDto);
    Task<MenuItemDto> UpdateAsync(int id, UpdateMenuItemDto menuItemDto);
    Task<IEnumerable<MenuItemDto>> GetMenuItemsByRestaurantIdAsync(int restaurantId);
}