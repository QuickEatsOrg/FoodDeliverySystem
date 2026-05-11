using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.API.DTOs.Sahil
{
    // When placing order
    public class CreateOrderDto
    {
        public int DeliveryAddressId { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = string.Empty;

        public string? CustomerRemarks { get; set; }
    }

    // When restaurant updates status
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = string.Empty;

        public string? Remarks { get; set; }
    }

    // Order item details
    public class OrderItemDto
    {
        public int MenuItemId { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }

    // Address in order
    public class OrderAddressDto
    {
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Landmark { get; set; }
    }

    // Complete order details
    public class OrderDto
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public int? DeliveryDriverId { get; set; }
        public string? DeliveryDriverName { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderAddressDto? DeliveryAddress { get; set; }
        public List<OrderItemDto>? Items { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? CustomerRemarks { get; set; }
        public bool CanCancel { get; set; }
    }
}
