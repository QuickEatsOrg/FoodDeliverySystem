namespace FoodDelivery.MVC.DTOs
{
    public class CustomerSummaryDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
    }

    public class DriverDto
    {
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? DriverVehicle { get; set; }
        public string? DriverEmail { get; set; }
    }
}
