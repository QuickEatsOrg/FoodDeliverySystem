using System;
using System.Collections.Generic;

namespace FoodDelivery.API.Models;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public string? RestaurantName { get; set; }

    public string? RestaurantAddress { get; set; }

    public string? RestaurantPhone { get; set; }

    public string? RestaurantEmail { get; set; }

    public string? RestaurantUnhashedPassword { get; set; }

    public string? RestaurantHashedPassword { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role Role { get; set; } = null!;

}
