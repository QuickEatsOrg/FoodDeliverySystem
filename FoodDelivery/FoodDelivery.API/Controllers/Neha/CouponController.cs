using FoodDelivery.API.DTOs.Neha;
using FoodDelivery.API.Services.Interfaces.Neha;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers.Neha
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        // url: api/coupon
        [HttpGet]
        public async Task<IActionResult> GetActiveCoupons()
        {
            var coupons = await _couponService.GetActiveCouponsAsync();

            return Ok(coupons);
        }

        // url: api/coupon/admin
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllCoupons()
        {
            var coupons = await _couponService.GetAllCouponsAsync();

            return Ok(coupons);
        }

        // url: api/coupon/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCouponById(int id)
        {
            var coupon = await _couponService.GetByIdAsync(id);

            return Ok(coupon);
        }

        // url: api/coupon
        [HttpPost]
        public async Task<IActionResult> AddCoupon(CreateCouponDto couponDto)
        {
            var result = await _couponService.AddAsync(couponDto);

            return Ok(result);
        }

        // url: api/coupon/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, UpdateCouponDto couponDto)
        {
            var result = await _couponService.UpdateAsync(id, couponDto);

            return Ok(result);
        }

        // url: api/coupon/validate?couponCode=SAVE10
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCoupon(string couponCode)
        {
            var result = await _couponService.ValidateCouponAsync(couponCode);

            return Ok("Coupon is valid");
        }

        // url: api/coupon/usage-stats
        [HttpGet("usage-stats")]
        public async Task<IActionResult> GetCouponUsageStats()
        {
            var stats = await _couponService.GetCouponUsageStatsAsync();

            return Ok(stats);
        }
    }
}