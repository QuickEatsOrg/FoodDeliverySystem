namespace FoodDelivery.MVC.DTOs
{
    public class RestaurantDto
    {
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; } = string.Empty;

        public string RestaurantAddress { get; set; } = string.Empty;

        public string RestaurantPhone { get; set; } = string.Empty;

        public string RestaurantEmail { get; set; } = string.Empty;
    }
}
