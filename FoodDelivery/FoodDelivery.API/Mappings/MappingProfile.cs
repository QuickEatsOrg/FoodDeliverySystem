using AutoMapper;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.DTOs.Neha;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Model -> DTO
            CreateMap<Coupon, CouponDtos>();
            // DTO -> Model
            CreateMap<CreateCouponDto, Coupon>();
            CreateMap<UpdateCouponDto, Coupon>();

            CreateMap<Rating, RatingDto>();

            CreateMap<CreateRatingDto, Rating>();

            CreateMap<UpdateRatingDto, Rating>();
        }
    }
}