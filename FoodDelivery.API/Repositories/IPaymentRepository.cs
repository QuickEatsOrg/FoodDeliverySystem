using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Repositories
{
    public interface IPaymentRepository
    {
        Task<PaymentDto?> GetByIdAsync(int id);
        Task<PaymentDto?> GetByOrderIdAsync(int orderId);
        Task<List<PaymentDto>> GetByUserIdAsync(int userId);
        Task<List<PaymentDto>> GetAllAsync();
        Task<PaymentDto> CreateAsync(PaymentDto payment);
        Task<PaymentDto> UpdateAsync(PaymentDto payment);
        Task<PaymentDto?> UpdateStatusAsync(int paymentId, string status, string transactionId);
    }
}
