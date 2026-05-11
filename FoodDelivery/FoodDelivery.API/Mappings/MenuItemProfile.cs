using FoodDelivery.API.Models;
using FoodDelivery.API.DTOs;
using AutoMapper;

public class MenuItemProfile : Profile
{
    public MenuItemProfile()
    {
        CreateMap<MenuItem, MenuItemDto>();
        CreateMap<CreateMenuItemDto, MenuItem>();
        CreateMap<UpdateMenuItemDto, MenuItem>();
    }
}