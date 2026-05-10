using AutoMapper;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Models;


namespace FoodDelivery.API.Mapper
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>().ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
                .ForMember(dest => dest.Addresses, opt => opt.Ignore());
        
        }
    }
}
