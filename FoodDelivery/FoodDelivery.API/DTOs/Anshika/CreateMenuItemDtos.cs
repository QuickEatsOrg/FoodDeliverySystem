using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.API.DTOs;

public class CreateMenuItemDto
{
    public string ItemName { get; set; }

    public string? ItemDescription { get; set; }

    public decimal ItemPrice { get; set; }

    public int RestaurantId { get; set; }
}