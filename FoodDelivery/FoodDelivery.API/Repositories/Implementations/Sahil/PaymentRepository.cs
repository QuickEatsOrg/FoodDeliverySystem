using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.API.DTOs.Sahil;
using FoodDelivery.API.Repositories.Interfaces.Sahil;

namespace FoodDelivery.API.Repositories.Implementations.Sahil
{
    /// <summary>
    /// Payment Repository - in-memory placeholder.
    /// No Payments table exists in the database yet.
    /// This allows the full API to compile and respond correctly.
    /// Replace with EF Core implementation once Payments table is added.
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private static readonly List<PaymentDto> _payments = new();
        private static int _nextId = 1;

        public Task<PaymentDto?> GetByIdAsync(int id)
        {
            var p = _payments.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(p);
        }

        public Task<PaymentDto?> GetByOrderIdAsync(int orderId)
        {
            var p = _payments.FirstOrDefault(x => x.OrderId == orderId);
            return Task.FromResult(p);
        }

        public Task<List<PaymentDto>> GetByUserIdAsync(int userId)
        {
            var result = _payments.OrderByDescending(p => p.CreatedAt).ToList();
            return Task.FromResult(result);
        }

        public Task<List<PaymentDto>> GetAllAsync()
        {
            var result = _payments.OrderByDescending(p => p.CreatedAt).ToList();
            return Task.FromResult(result);
        }

        public Task<PaymentDto> CreateAsync(PaymentDto payment)
        {
            payment.Id = _nextId++;
            payment.CreatedAt = DateTime.UtcNow;
            _payments.Add(payment);
            return Task.FromResult(payment);
        }

        public Task<PaymentDto> UpdateAsync(PaymentDto payment)
        {
            var idx = _payments.FindIndex(p => p.Id == payment.Id);
            if (idx >= 0) _payments[idx] = payment;
            return Task.FromResult(payment);
        }

        public Task<PaymentDto?> UpdateStatusAsync(int paymentId, string status, string transactionId)
        {
            var p = _payments.FirstOrDefault(x => x.Id == paymentId);
            if (p != null)
            {
                p.PaymentStatus = status;
                p.TransactionId = transactionId;
                if (status == "Success") p.PaidAt = DateTime.UtcNow;
            }
            return Task.FromResult(p);
        }
    }
}
