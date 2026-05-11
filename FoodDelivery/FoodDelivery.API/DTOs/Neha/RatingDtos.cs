namespace FoodDelivery.API.DTOs.Neha
{
    public class RatingDto
    {
        public int RatingId { get; set; }

        public int? OrderId { get; set; }

        public int? RestaurantId { get; set; }

        public int? Rating1 { get; set; }

        public string? Review { get; set; }
    }
}