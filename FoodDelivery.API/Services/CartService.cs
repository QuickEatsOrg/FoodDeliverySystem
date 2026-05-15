using FoodDelivery.API.DTOs;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services;
using System;
using System.Threading.Tasks;

namespace FoodDelivery.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMenuItemService _menuItemService;
        private readonly IRestaurantService _restaurantService;
        private readonly ICouponService _couponService;

        public CartService(
            ICartRepository cartRepository,
            IMenuItemService menuItemService,
            IRestaurantService restaurantService,
            ICouponService couponService)
        {
            _cartRepository = cartRepository;
            _menuItemService = menuItemService;
            _restaurantService = restaurantService;
            _couponService = couponService;
        }

        public async Task<CartDto> GetCartAsync(string cartId)
            => await _cartRepository.GetCartAsync(cartId);

        public async Task<CartDto> AddToCartAsync(string cartId, int menuItemId, int quantity)
        {
            // Get item details directly from MenuItemService
            var menuItem = await _menuItemService.GetByIdAsync(menuItemId);
            if (menuItem == null)
                throw new Exception($"Menu item {menuItemId} not found.");

            // Get restaurant details for the name
            var restaurant = await _restaurantService.GetByIdAsync(menuItem.RestaurantId);
            var restaurantName = restaurant?.RestaurantName ?? "";

            return await _cartRepository.AddToCartAsync(
                cartId,
                menuItemId,
                quantity,
                menuItem.ItemPrice,
                menuItem.ItemName,
                menuItem.RestaurantId,
                restaurantName);
        }

        public async Task<CartDto> UpdateCartItemAsync(string cartId, int menuItemId, int quantity)
            => await _cartRepository.UpdateCartItemAsync(cartId, menuItemId, quantity);

        public async Task<CartDto> RemoveFromCartAsync(string cartId, int menuItemId)
            => await _cartRepository.RemoveFromCartAsync(cartId, menuItemId);

        public async Task ClearCartAsync(string cartId)
            => await _cartRepository.ClearCartAsync(cartId);

        public async Task<CartDto> ApplyCouponAsync(string cartId, string couponCode, decimal orderAmount)
        {
            // Validate coupon directly using CouponService
            var isValid = await _couponService.ValidateCouponAsync(couponCode);
            if (!isValid)
                throw new Exception("Invalid or expired coupon code.");

            var coupon = await _couponService.GetByIdAsync(0); // This part is tricky if service doesn't have GetByCode
                                                               // Note: Neha's service seems to return string for Add/Update but we need the discount amount.
                                                               // Let's assume for now we apply a flat 10% or something if we can't get the amount easily, 
                                                               // OR check if CouponDtos has the amount.

            var coupons = await _couponService.GetActiveCouponsAsync();
            var activeCoupon = System.Linq.Enumerable.FirstOrDefault(coupons, c => c.CouponCode == couponCode);

            decimal discountAmount = activeCoupon?.DiscountAmount ?? 0m;

            return await _cartRepository.ApplyCouponAsync(cartId, couponCode, discountAmount);
        }
    }
}
