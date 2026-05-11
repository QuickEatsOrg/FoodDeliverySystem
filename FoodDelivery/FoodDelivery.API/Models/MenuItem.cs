using System;
using System.Collections.Generic;

namespace FoodDelivery.API.Models;

public partial class MenuItem
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public string? ItemDescription { get; set; }

    public decimal? ItemPrice { get; set; }

    public int? RestaurantId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Restaurant? Restaurant { get; set; }
}
