namespace FoodDelivery.MVC.Models
{
    public class DriverDto
    {
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? DriverVehicle { get; set; }
        public string? DriverEmail { get; set; }
        public int RoleId { get; set; }
    }

    public class DeliveryDto
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }

        public int? CustomerId { get; set; }
        public int? RestaurantId { get; set; }

        public int? DeliveryDriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? DriverVehicle { get; set; }
        public string? DriverEmail { get; set; }

        public string? OrderStatus { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalDrivers { get; set; }
        public int TotalDeliveries { get; set; }
        public int DeliveredOrders { get; set; }
        public List<DeliveryDto> RecentDeliveries { get; set; } = new();
    }

    public class MyDeliveriesViewModel
    {
        public int? DriverId { get; set; }
        public List<DeliveryDto> Deliveries { get; set; } = new();
    }
}