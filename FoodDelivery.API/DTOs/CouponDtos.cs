namespace FoodDelivery.API.DTOs
{
    public class CouponDtos
    {
        public int CouponId { get; set; }

        public string? CouponCode { get; set; }

        public decimal? DiscountAmount { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }

    public class CreateCouponDto
    {
        public string? CouponCode { get; set; }

        public decimal? DiscountAmount { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }

    public class UpdateCouponDto
    {
        public string? CouponCode { get; set; }

        public decimal? DiscountAmount { get; set; }

        public DateOnly? ExpiryDate { get; set; }
    }
}