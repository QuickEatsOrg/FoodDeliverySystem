using AutoMapper;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Mapper;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterDriverDto, DeliveryDriver>();
        CreateMap<RegisterRestaurantDto, Restaurant>();
        CreateMap<CreateCustomerDto, Customer>();
    }
}
