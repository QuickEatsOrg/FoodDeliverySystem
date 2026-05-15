using System;
using System.Collections.Generic;

namespace FoodDelivery.API.DTOs
{
    // When customer adds item to cart
    public class AddToCartDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }

    // When customer updates quantity
    public class UpdateCartItemDto
    {
        public int Quantity { get; set; }
    }

    // When customer applies coupon
    public class ApplyCouponDto
    {
        public string? CouponCode { get; set; }
    }

    // One item in cart
    public class CartItemDto
    {
        public int MenuItemId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    // Complete cart object
    public class CartDto
    {
        public string? CartId { get; set; }
        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? AppliedCouponCode { get; set; }
        public bool IsEmpty { get; set; }
    }
}
