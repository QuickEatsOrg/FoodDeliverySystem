using FoodDelivery.API.Models;

namespace FoodDelivery.API.Repositories;

public interface IMenuItemRepository
{
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<MenuItem?> GetByIdAsync(int id);
    Task AddAsync(MenuItem menuItem);
    Task UpdateAsync(MenuItem menuItem);
    Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantIdAsync(int restaurantId);
}