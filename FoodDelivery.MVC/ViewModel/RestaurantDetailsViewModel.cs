namespace FoodDelivery.MVC.ViewModel
{
    public class RestaurantDetailsViewModel
    {
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; } = string.Empty;

        public string RestaurantAddress { get; set; } = string.Empty;

        public string RestaurantPhone { get; set; } = string.Empty;

        public string RestaurantEmail { get; set; } = string.Empty;

        public double? AverageRating { get; set; }

        public int RatingCount { get; set; }

        public List<MenuItemCardViewModel> MenuItems { get; set; } = new();

        public string? ErrorMessage { get; set; }
    }
}
