using FoodDelivery.API.DTOs.Sahil;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Sahil;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FoodDelivery.API.Controllers.Sahil
{
    /// <summary>
    /// PaymentController – payment initiation, status, history, and webhook.
    ///
    /// POST /api/payments/initiate           – initiate payment for an order
    /// GET  /api/payments/status/{orderId}   – payment status
    /// GET  /api/payments/history/{userId}   – payment history
    /// GET  /api/payments                    – all payments (admin)
    /// POST /api/payments/webhook            – gateway callback
    /// </summary>
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST /api/payments/initiate?userId=1
        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment(
            [FromBody] InitiatePaymentDto dto,
            [FromQuery] int userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (userId <= 0) return BadRequest(new { message = "userId is required." });
            try
            {
                var result = await _paymentService.InitiatePaymentAsync(dto, userId);
                return Ok(result);
            }
            catch (NotFoundException ex)   { return NotFound(new { message = ex.Message }); }
            catch (ForbiddenException ex)  { return StatusCode(403, new { message = ex.Message }); }
            catch (BadRequestException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex)           { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/payments/status/{orderId}?userId=1&role=Customer
        [HttpGet("status/{orderId:int}")]
        public async Task<IActionResult> GetPaymentStatus(
            int orderId,
            [FromQuery] int userId,
            [FromQuery] string role = "Customer")
        {
            try
            {
                var status = await _paymentService.GetPaymentStatusAsync(orderId, userId, role);
                return Ok(status);
            }
            catch (NotFoundException ex)   { return NotFound(new { message = ex.Message }); }
            catch (ForbiddenException ex)  { return StatusCode(403, new { message = ex.Message }); }
            catch (Exception ex)           { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/payments/history/{userId}
        [HttpGet("history/{userId:int}")]
        public async Task<IActionResult> GetPaymentHistory(int userId)
        {
            try   { return Ok(await _paymentService.GetPaymentHistoryAsync(userId)); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/payments  (admin)
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try   { return Ok(await _paymentService.GetAllPaymentsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // POST /api/payments/webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> PaymentWebhook([FromBody] PaymentWebhookDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Invalid webhook payload." });
            try
            {
                await _paymentService.ProcessPaymentWebhookAsync(dto);
                return Ok(new { message = "Webhook processed." });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }
    }
}
