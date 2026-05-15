namespace FoodDelivery.API.DTOs
{
    public class DriverLoginDto
    {
        public string DriverEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class DriverLoginResponseDto
    {
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverEmail { get; set; }
        public string Role { get; set; } = "DeliveryDriver";
        public string Token { get; set; } = null!;
    }
}