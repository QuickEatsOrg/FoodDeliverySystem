using AutoMapper;
using FoodDelivery.API.DTOs.Neha;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services.Interfaces.Neha;
using FoodService.Exceptions;

namespace FoodDelivery.API.Services.Implementations.Neha
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        private readonly IMapper _mapper;

        public CouponService(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CouponDtos>> GetActiveCouponsAsync()
        {
            var coupons = await _couponRepository.GetActiveCouponsAsync();

            return _mapper.Map<IEnumerable<CouponDtos>>(coupons);
        }

        public async Task<IEnumerable<CouponDtos>> GetAllCouponsAsync()
        {
            var coupons = await _couponRepository.GetAllCouponsAsync();

            return _mapper.Map<IEnumerable<CouponDtos>>(coupons);
        }

        public async Task<CouponDtos?> GetByIdAsync(int couponId)
        {
            var coupon = await _couponRepository.GetByIdAsync(couponId);

            if (coupon == null)
            {
                throw new NotFoundException("Coupon not found");
            }

            return _mapper.Map<CouponDtos>(coupon);
        }

        public async Task<bool> ValidateCouponAsync(string couponCode)
        {
            if (string.IsNullOrWhiteSpace(couponCode))
            {
                throw new BadRequestException("Coupon code is required");
            }

            var isValid = await _couponRepository.ValidateCouponAsync(couponCode);

            if (!isValid)
            {
                throw new BadRequestException("Invalid coupon code");
            }

            return true;
        }

        public async Task<string> AddAsync(CreateCouponDto couponDto)
        {
            var coupon = _mapper.Map<Coupon>(couponDto);

            coupon.CouponCode = coupon.CouponCode.ToUpper();

            var existingCoupon = await _couponRepository
                .GetByCodeAsync(coupon.CouponCode);

            if (existingCoupon != null)
            {
                throw new BadRequestException("Coupon code already exists");
            }

            coupon.CouponId = await _couponRepository.GetNextCouponIdAsync();

            await _couponRepository.AddAsync(coupon);

            await _couponRepository.SaveChangesAsync();

            return "Coupon added successfully";
        }

        public async Task<string> UpdateAsync(int couponId, UpdateCouponDto couponDto)
        {
            var existingCoupon = await _couponRepository.GetByIdAsync(couponId);

            if (existingCoupon == null)
            {
                throw new NotFoundException("Coupon not found");
            }

            existingCoupon.CouponCode = couponDto.CouponCode.ToUpper();

            existingCoupon.DiscountAmount = couponDto.DiscountAmount;

            existingCoupon.ExpiryDate = couponDto.ExpiryDate;

            await _couponRepository.UpdateAsync(existingCoupon);

            await _couponRepository.SaveChangesAsync();

            return "Coupon updated successfully";
        }

        public async Task<object> GetCouponUsageStatsAsync()
        {
            return await _couponRepository.GetCouponUsageStatsAsync();
        }
    }
}