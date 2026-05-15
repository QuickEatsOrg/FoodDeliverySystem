using AutoMapper;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Coupon, CouponDtos>();
            CreateMap<CreateCouponDto, Coupon>();
            CreateMap<UpdateCouponDto, Coupon>();


            CreateMap<Rating, RatingDto>();
            CreateMap<CreateRatingDto, Rating>();
            CreateMap<UpdateRatingDto, Rating>();


            CreateMap<CreateAddressDto, DeliveryAddress>();
            CreateMap<UpdateAddressDto, DeliveryAddress>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<DeliveryAddress, AddressResponseDto>()
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.AddressId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(
            dest => dest.RestaurantHashedPassword,
            opt => opt.Ignore()
        );

            CreateMap<Restaurant, RestaurantDto>();

            CreateMap<UpdateRestaurantDto, Restaurant>();

            CreateMap<MenuItem, MenuItemDto>()
                .ForMember(
                    dest => dest.RestaurantName,
                    opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.RestaurantName : string.Empty)
                )
                .ForMember(
                    dest => dest.RestaurantAddress,
                    opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.RestaurantAddress : string.Empty)
                );
            CreateMap<CreateMenuItemDto, MenuItem>();
            CreateMap<UpdateMenuItemDto, MenuItem>();

            
            CreateMap<RegisterDriverDto, DeliveryDriver>()
                .ForMember(dest => dest.DriverHashedPassword, opt => opt.Ignore())
                .ForMember(dest => dest.DriverUnhashedPassword, opt => opt.Ignore());

            
            CreateMap<RegisterRestaurantDto, Restaurant>()
                .ForMember(dest => dest.RestaurantHashedPassword, opt => opt.Ignore())
                .ForMember(dest => dest.RestaurantUnhashedPassword, opt => opt.Ignore());

            
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.CustomerHashedPassword, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerUnhashedPassword, opt => opt.Ignore());

            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
                .ForMember(dest => dest.Addresses, opt => opt.Ignore());
        }
    }
}

//using AutoMapper;
//using FoodDelivery.API.DTOs;
//using FoodDelivery.API.Models;

//namespace FoodDelivery.API.Mappings;

//public class MappingProfile : Profile
//{
//    public MappingProfile()
//    {
//        // Auth & User Registration Mappings
//        CreateMap<CreateCustomerDto, Customer>()
//            .ForMember(dest => dest.CustomerUnhashedPassword, opt => opt.MapFrom(src => src.Password))
//            .ForMember(dest => dest.CustomerHashedPassword, opt => opt.Ignore());

//        CreateMap<RegisterDriverDto, DeliveryDriver>()
//            .ForMember(dest => dest.DriverUnhashedPassword, opt => opt.Ignore())
//            .ForMember(dest => dest.DriverHashedPassword, opt => opt.Ignore());

//        CreateMap<RegisterRestaurantDto, Restaurant>()
//           .ForMember(dest => dest.RestaurantUnhashedPassword, opt => opt.Ignore())
//           .ForMember(dest => dest.RestaurantHashedPassword, opt => opt.Ignore());

//        // Customer Mappings
//        CreateMap<Customer, CustomerResponseDto>()
//            .ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
//            .ForMember(dest => dest.Addresses, opt => opt.Ignore());

//        // Address Mappings
//        CreateMap<CreateAddressDto, DeliveryAddress>();
//        CreateMap<UpdateAddressDto, DeliveryAddress>()
//            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
//        CreateMap<DeliveryAddress, AddressResponseDto>();

//        // Restaurant Mappings
//        CreateMap<CreateRestaurantDto, Restaurant>()
//            .ForMember(dest => dest.RestaurantHashedPassword, opt => opt.Ignore());
//        CreateMap<Restaurant, RestaurantDto>();
//        CreateMap<UpdateRestaurantDto, Restaurant>();

//        // MenuItem Mappings
//        CreateMap<MenuItem, MenuItemDto>();
//        CreateMap<CreateMenuItemDto, MenuItem>();
//        CreateMap<UpdateMenuItemDto, MenuItem>();

//        // Coupon Mappings
//        CreateMap<Coupon, CouponDtos>();
//        CreateMap<CreateCouponDto, Coupon>();
//        CreateMap<UpdateCouponDto, Coupon>();

//        // Rating Mappings
//        CreateMap<Rating, RatingDto>();
//        CreateMap<CreateRatingDto, Rating>();
//        CreateMap<UpdateRatingDto, Rating>();
//    }
//}
