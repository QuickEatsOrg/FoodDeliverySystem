using AutoMapper;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Mappings;

public class RestaurantProfile: Profile
{
    public RestaurantProfile()
    {
        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(
            dest => dest.RestaurantHashedPassword,
            opt => opt.Ignore()
        );

        CreateMap<Restaurant, RestaurantDto>();

        CreateMap<UpdateRestaurantDto, Restaurant>();

        CreateMap<MenuItem, MenuItemDto>();
    }
}

