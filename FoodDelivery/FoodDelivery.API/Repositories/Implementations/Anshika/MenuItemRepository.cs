using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly FoodDeliveryDbContext _context;

    public MenuItemRepository(FoodDeliveryDbContext dbcontext)
    {
        _context = dbcontext;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems.ToListAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _context.MenuItems.FindAsync(id);
    }

    public async Task AddAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        _context.MenuItems.Update(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantIdAsync(int restaurantId)
    {
        return await _context.MenuItems
            .Where(m => m.RestaurantId == restaurantId)
            .ToListAsync();
    }

}