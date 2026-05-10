using FoodDelivery.API.DTOs;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Models;
using AutoMapper;
using BCrypt.Net;
using FoodDelivery.API.Exceptions;

namespace FoodDelivery.API.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restRepo;
    private readonly IMapper _mapper;

    public RestaurantService(IRestaurantRepository restRepo, IMapper mapper)
    {
        _restRepo = restRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
    {
        var restaurants = await _restRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    public async Task<RestaurantDto> GetByIdAsync(int id)
    {
        var restaurant = await _restRepo.GetByIdAsync(id);
        if (restaurant == null) {
            throw new NotFoundException("Restaurant not found.");
        }

        return _mapper.Map<RestaurantDto>(restaurant);
    }

    public async Task<RestaurantDto> CreateAsync(CreateRestaurantDto restaurantDto)
    {
        var restaurant = _mapper.Map<Restaurant>(restaurantDto);

        restaurant.RestaurantUnhashedPassword = restaurantDto.Password;

        restaurant.RestaurantHashedPassword =
            BCrypt.Net.BCrypt.HashPassword(restaurantDto.Password);

        restaurant.RoleId = 3;

        await _restRepo.AddAsync(restaurant);

        return _mapper.Map<RestaurantDto>(restaurant);
   }

    public async Task<RestaurantDto> UpdateAsync(int id, UpdateRestaurantDto restaurantDto)
    {
        var restaurant = await _restRepo.GetByIdAsync(id);
        if (restaurant == null) {
            throw new NotFoundException("Restaurant not found.");
        }

        _mapper.Map(restaurantDto, restaurant);

        await _restRepo.UpdateAsync(restaurant);

        return _mapper.Map<RestaurantDto>(restaurant);
    }
    
    public async Task<IEnumerable<MenuItemDto>> GetByRestaurantIdAsync(int restaurantId)
    {
        var items = await _restRepo.GetByRestaurantIdAsync(restaurantId);
        return _mapper.Map<IEnumerable<MenuItemDto>>(items);
    }
    
}