namespace FoodDelivery.API.DTOs;

public class MenuItemDto
{
    public int ItemId { get; set; }

    public string ItemName { get; set; }

    public string ItemDescription { get; set; }

    public decimal ItemPrice { get; set; }

    public int RestaurantId { get; set; }
}

