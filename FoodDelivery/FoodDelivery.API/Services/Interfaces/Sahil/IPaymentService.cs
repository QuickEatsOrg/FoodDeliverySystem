using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDelivery.API.DTOs.Sahil;

namespace FoodDelivery.API.Services.Interfaces.Sahil
{
    public interface IPaymentService
    {
        Task<InitiatePaymentResponseDto> InitiatePaymentAsync(InitiatePaymentDto dto, int userId);
        Task<PaymentStatusDto> GetPaymentStatusAsync(int orderId, int userId, string role);
        Task<List<PaymentDto>> GetPaymentHistoryAsync(int userId);
        Task<List<PaymentDto>> GetAllPaymentsAsync();
        Task ProcessPaymentWebhookAsync(PaymentWebhookDto dto);
    }
}
