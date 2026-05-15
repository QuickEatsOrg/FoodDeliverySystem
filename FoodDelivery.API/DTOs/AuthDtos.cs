namespace FoodDelivery.API.DTOs;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Admin, Customer, DeliveryDriver, Restaurant
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class RegisterDriverDto
{
    public string DriverName { get; set; } = string.Empty;
    public string DriverPhone { get; set; } = string.Empty;
    public string DriverVehicle { get; set; } = string.Empty;
    public string DriverEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRestaurantDto
{
    public string RestaurantName { get; set; } = string.Empty;
    public string RestaurantAddress { get; set; } = string.Empty;
    public string RestaurantPhone { get; set; } = string.Empty;
    public string RestaurantEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
