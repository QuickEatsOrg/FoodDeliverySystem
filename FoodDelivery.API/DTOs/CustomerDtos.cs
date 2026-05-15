using FoodDelivery.API.DTOs;

public class    CreateCustomerDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateCustomerDto
{
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string? Password { get; set; }
}

public class CustomerResponseDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public List<AddressResponseDto> Addresses { get; set; } = new();
}