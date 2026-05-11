namespace FoodDelivery.API.DTOs.Neha
{
    public class CouponDtos
    {
        public int CouponId { get; set; }

        public string? CouponCode { get; set; }

        public decimal? DiscountAmount { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }
}