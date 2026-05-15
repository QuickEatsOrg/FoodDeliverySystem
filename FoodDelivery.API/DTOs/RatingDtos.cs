namespace FoodDelivery.API.DTOs
{
    public class RatingDto
    {
        public int RatingId { get; set; }

        public int? OrderId { get; set; }

        public int? RestaurantId { get; set; }

        public int? Rating1 { get; set; }

        public string? Review { get; set; }
    }

    public class AverageRatingDto
    {
        public int RestaurantId { get; set; }
        public double AverageRating { get; set; }
    }

    public class CreateRatingDto
    {
        public int? OrderId { get; set; }
        public int? RestaurantId { get; set; }
        public int? Rating1 { get; set; }
        public string? Review { get; set; }
    }

    public class UpdateRatingDto
    {
        public int? Rating1 { get; set; }
        public string? Review { get; set; }
    }
}