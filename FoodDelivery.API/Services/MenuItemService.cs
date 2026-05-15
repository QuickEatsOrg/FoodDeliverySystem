using FoodDelivery.API.DTOs;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Models;
using AutoMapper;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Exceptions;

namespace FoodDelivery.API.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepo;

    private readonly IMapper _mapper;

    public MenuItemService(IMenuItemRepository menuItemRepo, IMapper mapper)
    {
        _menuItemRepo = menuItemRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MenuItemDto>> GetAllAsync()
    {
        var menuItems = await _menuItemRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);
    }

    public async Task<MenuItemDto> GetByIdAsync(int id)
    {
        var menuItem = await _menuItemRepo.GetByIdAsync(id);

        if (menuItem == null)
        {
            throw new NotFoundException(
                "Menu item not found."
            );
        }

        return _mapper.Map<MenuItemDto>(menuItem);
    }

    public async Task<MenuItemDto> CreateAsync(CreateMenuItemDto menuItemDto)
    {
        var menuItem = _mapper.Map<MenuItem>(menuItemDto);
        await _menuItemRepo.AddAsync(menuItem);
        return _mapper.Map<MenuItemDto>(menuItem);
    }

    public async Task<MenuItemDto> UpdateAsync(int id, UpdateMenuItemDto menuItemDto)
    {
        var menuItem = await _menuItemRepo.GetByIdAsync(id);

        if (menuItem == null)
        {
            throw new NotFoundException(
                "Menu item not found."
            );
        }

        _mapper.Map(menuItemDto, menuItem);

        await _menuItemRepo.UpdateAsync(menuItem);

        return _mapper.Map<MenuItemDto>(menuItem);
    }

    public async Task DeleteAsync(int id)
    {
        var menuItem = await _menuItemRepo.GetByIdAsync(id);

        if (menuItem == null)
        {
            throw new NotFoundException("Menu item not found.");
        }

        await _menuItemRepo.DeleteAsync(menuItem);
    }

    public async Task<IEnumerable<MenuItemDto>> GetMenuItemsByRestaurantIdAsync(int restaurantId)
    {
        var menuItems = await _menuItemRepo.GetMenuItemsByRestaurantIdAsync(restaurantId);
        return _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);
    }
}
