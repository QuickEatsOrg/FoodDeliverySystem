using System;

namespace FoodDelivery.API.DTOs.Sahil
{
    // Start payment
    public class InitiatePaymentDto
    {
        public int OrderId { get; set; }
        public string? PaymentMethod { get; set; }
    }

    // Payment details output
    public class PaymentDto
    {
        public int Id { get; set; }
        public string? PaymentNumber { get; set; }
        public int OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Quick status check
    public class PaymentStatusDto
    {
        public int OrderId { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }
    }

    // Response after initiating payment
    public class InitiatePaymentResponseDto
    {
        public string? PaymentNumber { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? RedirectUrl { get; set; }
    }

    // Gateway webhook callback
    public class PaymentWebhookDto
    {
        public string? TransactionId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Signature { get; set; }
        public decimal Amount { get; set; }
        public string? OrderNumber { get; set; }
    }
}
