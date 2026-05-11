using FoodDelivery.API.DTOs.Sahil;
using FoodDelivery.API.Repositories.Interfaces.Sahil;
using FoodDelivery.API.Services.Interfaces.Sahil;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodDelivery.API.Services.Implementations.Sahil
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public CartService(ICartRepository cartRepository, IHttpClientFactory httpClientFactory)
        {
            _cartRepository = cartRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CartDto> GetCartAsync(string cartId)
            => await _cartRepository.GetCartAsync(cartId);

        public async Task<CartDto> AddToCartAsync(string cartId, int menuItemId, int quantity)
        {
            // Call Menu API to get item details
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7002/api/menu/{menuItemId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Menu item {menuItemId} not found.");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var price          = root.GetProperty("price").GetDecimal();
            var itemName       = root.GetProperty("name").GetString() ?? "Item";
            var restaurantId   = root.GetProperty("restaurantId").GetInt32();
            var restaurantName = root.GetProperty("restaurantName").GetString() ?? "";

            return await _cartRepository.AddToCartAsync(cartId, menuItemId, quantity, price, itemName, restaurantId, restaurantName);
        }

        public async Task<CartDto> UpdateCartItemAsync(string cartId, int menuItemId, int quantity)
            => await _cartRepository.UpdateCartItemAsync(cartId, menuItemId, quantity);

        public async Task<CartDto> RemoveFromCartAsync(string cartId, int menuItemId)
            => await _cartRepository.RemoveFromCartAsync(cartId, menuItemId);

        public async Task ClearCartAsync(string cartId)
            => await _cartRepository.ClearCartAsync(cartId);

        public async Task<CartDto> ApplyCouponAsync(string cartId, string couponCode, decimal orderAmount)
        {
            // Call Coupon API to validate
            var client  = _httpClientFactory.CreateClient();
            var payload = $"{{\"couponCode\":\"{couponCode}\",\"orderAmount\":{orderAmount}}}";
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7006/api/coupons/validate", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Invalid or expired coupon code.");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var discountAmount = doc.RootElement.GetProperty("discountAmount").GetDecimal();

            return await _cartRepository.ApplyCouponAsync(cartId, couponCode, discountAmount);
        }
    }
}
