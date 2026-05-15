using System;

namespace FoodDelivery.API.DTOs
{
    public class AssignDriverDto
    {
        public int OrderId { get; set; }
        public int DriverId { get; set; }
    }

    public class UpdateDeliveryStatusDto
    {
        public string OrderStatus { get; set; } = null!;
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
}