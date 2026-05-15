namespace FoodDelivery.MVC.ViewModel
{
    public class MenuItemDetailsViewModel
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string ItemDescription { get; set; } = string.Empty;

        public decimal ItemPrice { get; set; }

        public string CuisineCategory { get; set; } = "Food";

        public int Quantity { get; set; }

        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; } = string.Empty;

        public string RestaurantAddress { get; set; } = string.Empty;

        public string RestaurantPhone { get; set; } = string.Empty;

        public string RestaurantEmail { get; set; } = string.Empty;

        public double? AverageRating { get; set; }

        public int RatingCount { get; set; }

        public List<MenuItemCardViewModel> OtherMenuItems { get; set; } = new();

        public string? ErrorMessage { get; set; }
    }
}
