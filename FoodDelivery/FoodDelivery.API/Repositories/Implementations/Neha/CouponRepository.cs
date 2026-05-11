using FoodDelivery.API.Data;
using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories.Implementations
{
    public class CouponRepository : ICouponRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public CouponRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coupon>> GetActiveCouponsAsync()
        {
            return await _context.Coupons
                .Where(c => c.ExpiryDate >= DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
        }

        public async Task<Coupon?> GetByIdAsync(int couponId)
        {
            return await _context.Coupons.FindAsync(couponId);
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task<Coupon?> GetByCodeAsync(string couponCode)
        {
            return await _context.Coupons
                .FirstOrDefaultAsync(c => c.CouponCode == couponCode);
        }

        public async Task<bool> ValidateCouponAsync(string couponCode)
        {
            return await _context.Coupons.AnyAsync(c =>
                c.CouponCode == couponCode &&
                c.ExpiryDate >= DateOnly.FromDateTime(DateTime.Now));
        }

        public async Task<int> GetNextCouponIdAsync()
        {
            if (!await _context.Coupons.AnyAsync())
            {
                return 1;
            }

            return await _context.Coupons.MaxAsync(c => c.CouponId) + 1;
        }
        public async Task AddAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
        }

        public Task UpdateAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}