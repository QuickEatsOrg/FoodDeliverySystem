using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly FoodDeliveryDbContext _context;

    public RestaurantRepository(FoodDeliveryDbContext dbcontext)
    {
        _context = dbcontext;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await _context.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await _context.Restaurants.FindAsync(id);
    }

    public async Task AddAsync(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        _context.Restaurants.Update(restaurant);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MenuItem>> GetByRestaurantIdAsync(int restaurantId)
    {
    return await _context.MenuItems
        .Where(m => m.RestaurantId == restaurantId)
        .ToListAsync();
    }
    
}