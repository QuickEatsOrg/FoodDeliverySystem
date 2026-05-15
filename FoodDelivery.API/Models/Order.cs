using System;
using System.Collections.Generic;

namespace FoodDelivery.API.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? CustomerId { get; set; }

    public int? RestaurantId { get; set; }

    public int? DeliveryDriverId { get; set; }

    public string? OrderStatus { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual DeliveryDriver? DeliveryDriver { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Restaurant? Restaurant { get; set; }

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
}
