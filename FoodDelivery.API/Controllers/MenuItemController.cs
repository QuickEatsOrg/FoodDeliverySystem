using FoodDelivery.API.DTOs;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetAll()
    {
        var menuItems = await _menuItemService.GetAllAsync();
        return Ok(menuItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemDto>> GetById(int id)
    {
        var menuItem = await _menuItemService.GetByIdAsync(id);

        return Ok(menuItem);
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemDto>> Create(CreateMenuItemDto menuItemDto)
    {
        var createdmenuItem = await _menuItemService.CreateAsync(menuItemDto);

        return CreatedAtAction(
           nameof(GetById),
           new { id = createdmenuItem.ItemId },
           createdmenuItem
        );
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<MenuItemDto>> Update(int id, UpdateMenuItemDto menuItem)
    {
        var updatedMenuItem = await _menuItemService.UpdateAsync(id, menuItem);

        return Ok(updatedMenuItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _menuItemService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("restaurant/{restaurantId}")]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItemsByRestaurantIdAsync(int restaurantId)
    {
        var menuItems = await _menuItemService.GetMenuItemsByRestaurantIdAsync(restaurantId);
        return Ok(menuItems);
    }
}
