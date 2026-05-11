using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FoodDelivery.API.DTOs.Sahil;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Repositories.Interfaces.Sahil;
using FoodDelivery.API.Services.Interfaces.Sahil;

namespace FoodDelivery.API.Services.Implementations.Sahil
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            IHttpClientFactory httpClientFactory)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<InitiatePaymentResponseDto> InitiatePaymentAsync(InitiatePaymentDto dto, int userId)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId);
            if (order == null)
                throw new NotFoundException("Order not found.");

            if (order.CustomerId != userId)
                throw new ForbiddenException("You can only pay for your own orders.");

            if (order.OrderStatus != "Pending")
                throw new BadRequestException("Order is not in a payable state.");

            var existing = await _paymentRepository.GetByOrderIdAsync(dto.OrderId);
            if (existing != null && existing.PaymentStatus == "Success")
                throw new BadRequestException("This order has already been paid.");

            // Create payment record
            var paymentNumber = $"PAY{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var newPayment = new PaymentDto
            {
                PaymentNumber = paymentNumber,
                OrderId       = dto.OrderId,
                Amount        = 0m,   // DB doesn't store TotalAmount on Order yet
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Pending",
                CreatedAt     = DateTime.UtcNow
            };

            var created = await _paymentRepository.CreateAsync(newPayment);

            // Simulate payment gateway success
            var transactionId = Guid.NewGuid().ToString("N");
            await _paymentRepository.UpdateStatusAsync(created.Id, "Success", transactionId);

            // Update order status
            await _orderRepository.UpdateStatusAsync(order.OrderId, "Confirmed");

            // Fire notification
            _ = SendNotification(userId, "Payment Successful",
                $"Payment for order #{dto.OrderId} was successful.", "Payment");

            return new InitiatePaymentResponseDto
            {
                PaymentNumber = paymentNumber,
                Amount        = 0m,
                PaymentStatus = "Success",
                TransactionId = transactionId,
                RedirectUrl   = $"/order/confirmation/{dto.OrderId}"
            };
        }

        public async Task<PaymentStatusDto> GetPaymentStatusAsync(int orderId, int userId, string role)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order not found.");

            if (role != "Admin" && order.CustomerId != userId)
                throw new ForbiddenException("You do not have permission to view this payment.");

            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
            if (payment == null)
                return new PaymentStatusDto { OrderId = orderId, PaymentStatus = "Not Initiated", Amount = 0m };

            return new PaymentStatusDto
            {
                OrderId       = orderId,
                PaymentStatus = payment.PaymentStatus,
                Amount        = payment.Amount,
                TransactionId = payment.TransactionId
            };
        }

        public async Task<List<PaymentDto>> GetPaymentHistoryAsync(int userId)
            => await _paymentRepository.GetByUserIdAsync(userId);

        public async Task<List<PaymentDto>> GetAllPaymentsAsync()
            => await _paymentRepository.GetAllAsync();

        public async Task ProcessPaymentWebhookAsync(PaymentWebhookDto dto)
        {
            // Placeholder: process gateway callback
            await Task.CompletedTask;
        }

        private async Task SendNotification(int userId, string title, string message, string type)
        {
            try
            {
                var client  = _httpClientFactory.CreateClient();
                var payload = $"{{\"userId\":{userId},\"title\":\"{title}\",\"message\":\"{message}\",\"type\":\"{type}\"}}";
                await client.PostAsync("https://localhost:7003/api/notifications",
                    new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));
            }
            catch { /* don't fail payment if notification fails */ }
        }
    }
}
