using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.API.DTOs;

public class CreateRestaurantDto
{
    public string RestaurantName { get; set; }

    public string RestaurantAddress { get; set; }

    public string RestaurantPhone { get; set; }

    public string RestaurantEmail { get; set; }

    public string Password { get; set; }
}