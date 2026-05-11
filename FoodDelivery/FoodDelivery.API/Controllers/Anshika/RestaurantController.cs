using FoodDelivery.API.DTOs;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var restaurants = await _restaurantService.GetAllAsync();
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetById(int id)
    {
        var restaurants = await _restaurantService.GetByIdAsync(id);

        return Ok(restaurants);
    }

    [HttpPost]
    public async Task<ActionResult<RestaurantDto>> Create(CreateRestaurantDto restaurantDto)
    {
        var createdRestaurant = await _restaurantService.CreateAsync(restaurantDto);

        return CreatedAtAction(
           nameof(GetById),
           new { id = createdRestaurant.RestaurantId },
           createdRestaurant
       );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RestaurantDto>> Update(int id, UpdateRestaurantDto restaurantDto)
    {
        var updatedRestaurant = await _restaurantService.UpdateAsync(id, restaurantDto);

        return Ok(updatedRestaurant);
    }

    [HttpGet("{id}/menuitems")]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems(int id)
    {
        var items = await _restaurantService.GetByRestaurantIdAsync(id);

        if (items == null || !items.Any())
            return NotFound();

        return Ok(items);
    }
}

