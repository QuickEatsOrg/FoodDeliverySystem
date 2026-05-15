namespace FoodDelivery.API.DTOs;

public class MenuItemDto
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string ItemDescription { get; set; } = string.Empty;


    public decimal ItemPrice { get; set; }

    public int RestaurantId { get; set; }

    public string RestaurantName { get; set; } = string.Empty;

    public string RestaurantAddress { get; set; } = string.Empty;
}

public class UpdateMenuItemDto
{
    public string ItemName { get; set; } = string.Empty;


    public string ItemDescription { get; set; } = string.Empty;

    public decimal ItemPrice { get; set; }

}
public class CreateMenuItemDto
{
    public string ItemName { get; set; } = string.Empty;

    public string? ItemDescription { get; set; }

    public decimal ItemPrice { get; set; }

    public int RestaurantId { get; set; }
}

