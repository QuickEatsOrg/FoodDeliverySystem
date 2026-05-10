using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly FoodDeliveryDbContext _dbcontext;

    public RestaurantRepository(FoodDeliveryDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await _dbcontext.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await _dbcontext.Restaurants.FindAsync(id);
    }

    public async Task AddAsync(Restaurant restaurant)
    {
        await _dbcontext.Restaurants.AddAsync(restaurant);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        _dbcontext.Restaurants.Update(restaurant);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<IEnumerable<MenuItem>> GetByRestaurantIdAsync(int restaurantId)
    {
    return await _dbcontext.MenuItems
        .Where(m => m.RestaurantId == restaurantId)
        .ToListAsync();
    }
    
}