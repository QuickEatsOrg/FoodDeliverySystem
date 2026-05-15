using FoodDelivery.API.DTOs;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FoodDelivery.API.Controllers
{
    /// <summary>
    /// CartController – session-based shopping cart management.
    ///
    /// GET    /api/cart/{cartId}                  – view cart
    /// POST   /api/cart/{cartId}/items            – add item
    /// PUT    /api/cart/{cartId}/items/{itemId}   – update quantity
    /// DELETE /api/cart/{cartId}/items/{itemId}   – remove item
    /// DELETE /api/cart/{cartId}                  – clear cart
    /// POST   /api/cart/{cartId}/coupon           – apply coupon
    /// </summary>
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET /api/cart/{cartId}
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart(string cartId)
        {
            try
            {
                var cart = await _cartService.GetCartAsync(cartId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST /api/cart/{cartId}/items
        // Body: { "menuItemId": 5, "quantity": 2 }
        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddToCart(string cartId, [FromBody] AddToCartDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var cart = await _cartService.AddToCartAsync(cartId, dto.MenuItemId, dto.Quantity);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/cart/{cartId}/items/{menuItemId}
        // Body: { "quantity": 3 }
        [HttpPut("{cartId}/items/{menuItemId:int}")]
        public async Task<IActionResult> UpdateCartItem(string cartId, int menuItemId, [FromBody] UpdateCartItemDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var cart = await _cartService.UpdateCartItemAsync(cartId, menuItemId, dto.Quantity);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/cart/{cartId}/items/{menuItemId}
        [HttpDelete("{cartId}/items/{menuItemId:int}")]
        public async Task<IActionResult> RemoveFromCart(string cartId, int menuItemId)
        {
            try
            {
                var cart = await _cartService.RemoveFromCartAsync(cartId, menuItemId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/cart/{cartId}
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> ClearCart(string cartId)
        {
            try
            {
                await _cartService.ClearCartAsync(cartId);
                return Ok(new { message = "Cart cleared." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST /api/cart/{cartId}/coupon
        // Body: { "couponCode": "SAVE10" }
        [HttpPost("{cartId}/coupon")]
        public async Task<IActionResult> ApplyCoupon(string cartId, [FromBody] ApplyCouponDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CouponCode))
                return BadRequest(new { message = "Coupon code is required." });
            try
            {
                var cart = await _cartService.ApplyCouponAsync(cartId, dto.CouponCode, 0m);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
