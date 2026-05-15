using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FoodDelivery.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

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

        [HttpGet("history/{userId:int}")]
        public async Task<IActionResult> GetPaymentHistory(int userId)
        {
            try   { return Ok(await _paymentService.GetPaymentHistoryAsync(userId)); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try   { return Ok(await _paymentService.GetAllPaymentsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

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
