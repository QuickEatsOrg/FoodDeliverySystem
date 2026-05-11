using System;
using System.Collections.Generic;

namespace FoodDelivery.API.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<DeliveryDriver> DeliveryDrivers { get; set; } = new List<DeliveryDriver>();

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
