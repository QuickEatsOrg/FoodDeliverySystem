using FoodDelivery.API.DTOs.Neha;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Services.Interfaces.Neha
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponDtos>> GetActiveCouponsAsync();

        Task<IEnumerable<CouponDtos>> GetAllCouponsAsync();

        Task<CouponDtos?> GetByIdAsync(int couponId);

        Task<bool> ValidateCouponAsync(string couponCode);
        //Task<int> GetNextCouponIdAsync();
        Task<string> AddAsync(CreateCouponDto couponDto);

        Task<string> UpdateAsync(int couponId, UpdateCouponDto couponDto);

        Task<object> GetCouponUsageStatsAsync();
    }
}