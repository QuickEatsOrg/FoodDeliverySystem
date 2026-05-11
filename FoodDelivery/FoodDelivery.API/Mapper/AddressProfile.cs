using AutoMapper;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Mapper
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<CreateAddressDto, DeliveryAddress>();
            CreateMap<UpdateAddressDto, DeliveryAddress>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<DeliveryAddress, AddressResponseDto>()
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.AddressId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));                
        }
    }
}
