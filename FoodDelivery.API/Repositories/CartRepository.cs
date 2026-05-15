using FoodDelivery.API.DTOs;
using FoodDelivery.API.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodDelivery.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;
        private string GetCartKey(string cartId) => $"Cart_{cartId}";

        public async Task<CartDto> GetCartAsync(string cartId)
        {
            var cartJson = Session.GetString(GetCartKey(cartId));
            if (string.IsNullOrEmpty(cartJson))
                return new CartDto { CartId = cartId, Items = new System.Collections.Generic.List<CartItemDto>(), IsEmpty = true };

            var cart = JsonSerializer.Deserialize<CartDto>(cartJson)!;
            cart.IsEmpty = cart.Items == null || cart.Items.Count == 0;
            return await Task.FromResult(cart);
        }

        public async Task<CartDto> AddToCartAsync(string cartId, int menuItemId, int quantity, decimal price, string itemName, int restaurantId, string restaurantName)
        {
            var cart = await GetCartAsync(cartId);

            if (cart.RestaurantId != 0 && cart.RestaurantId != restaurantId)
                throw new Exception("Cannot mix items from different restaurants. Clear your cart first.");

            cart.RestaurantId = restaurantId;
            cart.RestaurantName = restaurantName;

            var existing = cart.Items.FirstOrDefault(x => x.MenuItemId == menuItemId);
            if (existing != null)
            {
                existing.Quantity += quantity;
                existing.Subtotal = existing.Price * existing.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItemDto
                {
                    MenuItemId = menuItemId,
                    Name = itemName,
                    Price = price,
                    Quantity = quantity,
                    Subtotal = price * quantity
                });
            }

            await RecalculateCart(cart);
            await SaveCart(cart);
            return cart;
        }

        public async Task<CartDto> UpdateCartItemAsync(string cartId, int menuItemId, int quantity)
        {
            var cart = await GetCartAsync(cartId);
            var item = cart.Items.FirstOrDefault(x => x.MenuItemId == menuItemId);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Items.Remove(item);
                else
                {
                    item.Quantity = quantity;
                    item.Subtotal = item.Price * quantity;
                }
            }

            await RecalculateCart(cart);
            await SaveCart(cart);
            return cart;
        }

        public async Task<CartDto> RemoveFromCartAsync(string cartId, int menuItemId)
        {
            var cart = await GetCartAsync(cartId);
            var item = cart.Items.FirstOrDefault(x => x.MenuItemId == menuItemId);
            if (item != null) cart.Items.Remove(item);

            await RecalculateCart(cart);
            await SaveCart(cart);
            return cart;
        }

        public async Task ClearCartAsync(string cartId)
        {
            Session.Remove(GetCartKey(cartId));
            await Task.CompletedTask;
        }

        public async Task<CartDto> ApplyCouponAsync(string cartId, string couponCode, decimal discountAmount)
        {
            var cart = await GetCartAsync(cartId);
            cart.AppliedCouponCode = couponCode;
            cart.DiscountAmount = discountAmount;
            await RecalculateCart(cart);
            await SaveCart(cart);
            return cart;
        }

        private async Task RecalculateCart(CartDto cart)
        {
            cart.Subtotal    = cart.Items.Sum(x => x.Subtotal);
            cart.DeliveryFee = cart.Subtotal >= 500 ? 0 : 40;
            var taxable      = cart.Subtotal - cart.DiscountAmount;
            cart.TaxAmount   = Math.Max(0, taxable) * 5 / 100;
            cart.TotalAmount = cart.Subtotal + cart.TaxAmount + cart.DeliveryFee - cart.DiscountAmount;
            cart.IsEmpty     = cart.Items.Count == 0;
            await Task.CompletedTask;
        }

        private async Task SaveCart(CartDto cart)
        {
            Session.SetString(GetCartKey(cart.CartId!), JsonSerializer.Serialize(cart));
            await Task.CompletedTask;
        }
    }
}
