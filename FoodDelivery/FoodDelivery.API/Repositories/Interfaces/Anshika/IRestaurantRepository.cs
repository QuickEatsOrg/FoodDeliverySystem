using FoodDelivery.API.Models;

namespace FoodDelivery.API.Repositories;

public interface IRestaurantRepository
{
   Task<IEnumerable<Restaurant>> GetAllAsync();
   Task<Restaurant?> GetByIdAsync(int id);
   Task AddAsync(Restaurant restaurant);
   Task UpdateAsync(Restaurant restaurant);
   Task<IEnumerable<MenuItem>> GetByRestaurantIdAsync(int restaurantId);
}