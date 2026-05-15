using System.ComponentModel.DataAnnotations;
using FoodDelivery.MVC.DTOs;

namespace FoodDelivery.MVC.ViewModel
{
    public class CustomerDashboardViewModel
    {
        public string Name { get; set; } = "Customer";
        public int SavedItems { get; set; }
        public int ActiveOrders { get; set; }
    }

    public class RestaurantDashboardViewModel
    {
        public RestaurantDto? Restaurant { get; set; }
        public List<MenuItemCardViewModel> MenuItems { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }

    public class DriverDashboardViewModel
    {
        public string DriverName { get; set; } = "Delivery Driver";
        public List<DeliveryCardViewModel> Deliveries { get; set; } = new();
    }

    public class DeliveryCardViewModel
    {
        public int DeliveryId { get; set; }
        public string Pickup { get; set; } = "Restaurant pickup";
        public string Dropoff { get; set; } = "Customer address";
        public string Status { get; set; } = "Pending";
    }

    public class AdminDashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalRestaurants { get; set; }
        public int TotalDeliveryDrivers { get; set; }
        public int TotalOrders { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class DriverRegisterViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string DriverName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string DriverPhone { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Vehicle")]
        public string DriverVehicle { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string DriverEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class RestaurantCardViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string RestaurantAddress { get; set; } = string.Empty;
        public string CuisineCategory { get; set; } = "Food";
        public double? AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}
