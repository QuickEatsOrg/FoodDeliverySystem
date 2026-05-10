using System;
using System.Collections.Generic;

namespace FoodDelivery.API.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int? OrderId { get; set; }

    public int? RestaurantId { get; set; }

    public int? Rating1 { get; set; }

    public string? Review { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Restaurant? Restaurant { get; set; }
}
