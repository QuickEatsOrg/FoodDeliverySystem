using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly FoodDeliveryDbContext _dbcontext;

    public MenuItemRepository(FoodDeliveryDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        return await _dbcontext.MenuItems
            .Include(m => m.Restaurant)
            .ToListAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _dbcontext.MenuItems
            .Include(m => m.Restaurant)
            .FirstOrDefaultAsync(m => m.ItemId == id);
    }

    public async Task AddAsync(MenuItem menuItem)
    {
        await _dbcontext.MenuItems.AddAsync(menuItem);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        _dbcontext.MenuItems.Update(menuItem);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task DeleteAsync(MenuItem menuItem)
    {
        _dbcontext.MenuItems.Remove(menuItem);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantIdAsync(int restaurantId)
    {
        return await _dbcontext.MenuItems
            .Include(m => m.Restaurant)
            .Where(m => m.RestaurantId == restaurantId)
            .ToListAsync();
    }

}
