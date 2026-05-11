namespace FoodDelivery.API.DTOs.Neha
{
    public class UpdateCouponDto
    {
        public string? CouponCode { get; set; }

        public decimal? DiscountAmount { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }
}