using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    /// <summary>
    /// OrderController – manages the full order lifecycle.
    ///
    /// POST   /api/orders                             – place order from cart
    /// GET    /api/orders/{id}                        – get order by ID
    /// GET    /api/orders/my/{customerId}             – customer's own orders
    /// GET    /api/orders/restaurant/{restaurantId}   – orders for a restaurant
    /// GET    /api/orders/driver/{driverId}           – orders assigned to driver
    /// GET    /api/orders                             – all orders (admin)
    /// DELETE /api/orders/{id}/cancel/{customerId}    – cancel pending order
    /// PUT    /api/orders/{id}/status                 – update status (restaurant/admin)
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST /api/orders?customerId=1&cartId=abc
        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderDto dto,
            [FromQuery] int customerId,
            [FromQuery] string cartId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (customerId <= 0) return BadRequest(new { message = "customerId is required." });
            if (string.IsNullOrWhiteSpace(cartId)) return BadRequest(new { message = "cartId is required." });

            try
            {
                var order = await _orderService.CreateOrderAsync(dto, customerId, cartId);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (BadRequestException ex) { return BadRequest(new { message = ex.Message }); }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/orders/{id}?userId=1&role=Customer
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(
            int id,
            [FromQuery] int userId,
            [FromQuery] string role = "Customer")
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id, userId, role);
                return Ok(order);
            }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (ForbiddenException ex) { return StatusCode(403, new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/orders/my/{customerId}
        [HttpGet("my/{customerId:int}")]
        public async Task<IActionResult> GetMyOrders(int customerId)
        {
            try { return Ok(await _orderService.GetMyOrdersAsync(customerId)); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/orders/restaurant/{restaurantId}
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetRestaurantOrders(int restaurantId)
        {
            try { return Ok(await _orderService.GetRestaurantOrdersAsync(restaurantId)); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/orders/driver/{driverId}
        [HttpGet("driver/{driverId:int}")]
        public async Task<IActionResult> GetDriverOrders(int driverId)
        {
            try { return Ok(await _orderService.GetDriverOrdersAsync(driverId)); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // GET /api/orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try { return Ok(await _orderService.GetAllOrdersAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // DELETE /api/orders/{id}/cancel/{customerId}
        [HttpDelete("{id:int}/cancel/{customerId:int}")]
        public async Task<IActionResult> CancelOrder(int id, int customerId)
        {
            try
            {
                var ok = await _orderService.CancelOrderAsync(id, customerId);
                return ok ? Ok(new { message = "Order cancelled." }) : BadRequest(new { message = "Could not cancel order." });
            }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (ForbiddenException ex) { return StatusCode(403, new { message = ex.Message }); }
            catch (BadRequestException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        // PUT /api/orders/{id}/status?userId=3&role=Restaurant
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
            int id,
            [FromBody] UpdateOrderStatusDto dto,
            [FromQuery] int userId,
            [FromQuery] string role = "Restaurant")
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(id, dto, userId, role);
                return Ok(order);
            }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (ForbiddenException ex) { return StatusCode(403, new { message = ex.Message }); }
            catch (BadRequestException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }
    }
}
