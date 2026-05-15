namespace FoodDelivery.MVC.ViewModel
{
    public class MenuItemCardViewModel
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string ItemDescription { get; set; } = string.Empty;

        public decimal ItemPrice { get; set; }

        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; } = "Restaurant";

        public string RestaurantAddress { get; set; } = string.Empty;

        public string CuisineCategory { get; set; } = "Food";

        public double? AverageRating { get; set; }

        public int RatingCount { get; set; }

        public int Quantity { get; set; }
    }
}
