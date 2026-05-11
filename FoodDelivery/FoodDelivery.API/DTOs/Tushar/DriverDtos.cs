namespace FoodDelivery.API.DTOs.Tushar
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

    public class CreateDriverDto
    {
        public string DriverName { get; set; } = null!;
        public string DriverPhone { get; set; } = null!;
        public string DriverVehicle { get; set; } = null!;
        public string DriverEmail { get; set; } = null!;
    }

    public class UpdateDriverDto
    {
        public string DriverName { get; set; } = null!;
        public string DriverPhone { get; set; } = null!;
        public string DriverVehicle { get; set; } = null!;
        public string DriverEmail { get; set; } = null!;
    }
}