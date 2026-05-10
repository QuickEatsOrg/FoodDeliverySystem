using System;
using System.Collections.Generic;

namespace FoodDelivery.API.Models;

public partial class DeliveryDriver
{
    public int DriverId { get; set; }

    public string? DriverName { get; set; }

    public string? DriverPhone { get; set; }

    public string? DriverVehicle { get; set; }

    public string? DriverEmail { get; set; }

    public string? DriverUnhashedPassword { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
