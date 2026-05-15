using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services;
using FoodDelivery.API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "DeliveryDriver")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetDeliveryByOrderId(int orderId)
        {
            try
            {
                var delivery = await _deliveryService.GetDeliveryByOrderIdAsync(orderId);
                return Ok(delivery);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetDeliveriesByDriverId(int driverId)
        {
            var deliveries = await _deliveryService.GetDeliveriesByDriverIdAsync(driverId);
            return Ok(deliveries);
        }

        [HttpPost("assign-driver")]
        public async Task<IActionResult> AssignDriver([FromBody] AssignDriverDto dto)
        {
            try
            {
                var delivery = await _deliveryService.AssignDriverAsync(dto);
                return Ok(delivery);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(int orderId, [FromBody] UpdateDeliveryStatusDto dto)
        {
            try
            {
                var delivery = await _deliveryService.UpdateDeliveryStatusAsync(orderId, dto);
                return Ok(delivery);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}