using FoodDelivery.API.Models;
namespace FoodDelivery.API.Repositories;

public interface ICouponRepository
{
    Task<IEnumerable<Coupon>> GetActiveCouponsAsync();
    Task<Coupon?> GetByIdAsync(int couponId);
    Task<IEnumerable<Coupon>> GetAllCouponsAsync();
    Task<Coupon?> GetByCodeAsync(string couponCode);
    Task<bool> ValidateCouponAsync(string couponCode);
    Task<Object> GetCouponUsageStatsAsync();
    Task<int> GetNextCouponIdAsync();
    Task AddAsync(Coupon coupon);
    Task UpdateAsync(Coupon coupon);
    Task SaveChangesAsync();
}