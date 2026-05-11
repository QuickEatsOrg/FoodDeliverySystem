using AutoMapper;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Mapper;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.CustomerUnhashedPassword, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerHashedPassword, opt => opt.Ignore());

        CreateMap<RegisterDriverDto, DeliveryDriver>()
            .ForMember(dest => dest.DriverUnhashedPassword, opt => opt.Ignore())
            .ForMember(dest => dest.DriverHashedPassword, opt => opt.Ignore());


        CreateMap<RegisterRestaurantDto, Restaurant>()
           .ForMember(dest => dest.RestaurantUnhashedPassword, opt => opt.Ignore())
           .ForMember(dest => dest.RestaurantHashedPassword, opt => opt.Ignore());
                
    }
}
