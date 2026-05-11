using FoodDelivery.API.DTOs.Sahil;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Interfaces.Sahil;
using FoodDelivery.API.Services.Interfaces.Sahil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodDelivery.API.Services.Implementations.Sahil
{
    /// <summary>
    /// OrderService - works with the existing slim Order model.
    ///
    /// The current Order model only has:
    ///   OrderId, OrderDate, CustomerId, RestaurantId, DeliveryDriverId, OrderStatus
    ///
    /// Extended info (OrderNumber, Subtotal, etc.) is kept in _orderMeta dictionary
    /// since those columns don't exist in the DB yet.
    ///
    /// OrderItem model only has: OrderItemId, OrderId, ItemId, Quantity
    /// Item name/price is read from the MenuItem navigation property (oi.Item).
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICartService _cartService;
        private readonly IHttpClientFactory _httpClientFactory;

        // In-memory supplement for columns not yet in DB
        private static readonly Dictionary<int, OrderMeta> _orderMeta = new();

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            ICartService cartService,
            IHttpClientFactory httpClientFactory)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _cartService = cartService;
            _httpClientFactory = httpClientFactory;
        }

        // ──────────────────────────────────────────────────────────────────
        // CREATE ORDER
        // ──────────────────────────────────────────────────────────────────
        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, int customerId, string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart.IsEmpty)
                throw new BadRequestException("Cart is empty. Add items before placing an order.");

            // Build Order with only existing model properties
            var order = new Order
            {
                CustomerId   = customerId,
                RestaurantId = cart.RestaurantId,
                OrderDate    = DateTime.UtcNow,
                OrderStatus  = "Pending"
            };

            var created = await _orderRepository.CreateAsync(order);

            // Generate a simple order number
            var orderNumber = $"ORD{1000 + created.OrderId}";

            // Save extended info in memory
            _orderMeta[created.OrderId] = new OrderMeta
            {
                OrderNumber     = orderNumber,
                Subtotal        = cart.Subtotal,
                TaxAmount       = cart.TaxAmount,
                DeliveryFee     = cart.DeliveryFee,
                DiscountAmount  = cart.DiscountAmount,
                TotalAmount     = cart.TotalAmount,
                CustomerRemarks = dto.CustomerRemarks,
                PaymentMethod   = dto.PaymentMethod
            };

            // Create order items  (ItemId = MenuItemId, Quantity only – as per model)
            var orderItems = cart.Items.Select(i => new OrderItem
            {
                OrderId  = created.OrderId,
                ItemId   = i.MenuItemId,
                Quantity = i.Quantity
            }).ToList();

            await _orderItemRepository.CreateRangeAsync(orderItems);

            // Clear cart
            await _cartService.ClearCartAsync(cartId);

            // Fire-and-forget notification
            _ = SendNotification(customerId, "Order Placed",
                $"Your order {orderNumber} has been placed successfully.", "Order");

            return await BuildDtoAsync(created);
        }

        // ──────────────────────────────────────────────────────────────────
        // GET BY ID
        // ──────────────────────────────────────────────────────────────────
        public async Task<OrderDto> GetOrderByIdAsync(int orderId, int userId, string role)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order not found.");

            bool isAdmin    = role == "Admin";
            bool isCustomer = order.CustomerId == userId;
            bool isDriver   = order.DeliveryDriverId == userId;

            if (!isAdmin && !isCustomer && !isDriver)
                throw new ForbiddenException("You do not have permission to view this order.");

            return await BuildDtoAsync(order);
        }

        // ──────────────────────────────────────────────────────────────────
        // LISTS
        // ──────────────────────────────────────────────────────────────────
        public async Task<List<OrderDto>> GetMyOrdersAsync(int customerId)
            => await MapListAsync(await _orderRepository.GetByCustomerIdAsync(customerId));

        public async Task<List<OrderDto>> GetRestaurantOrdersAsync(int restaurantId)
            => await MapListAsync(await _orderRepository.GetByRestaurantIdAsync(restaurantId));

        public async Task<List<OrderDto>> GetDriverOrdersAsync(int driverId)
            => await MapListAsync(await _orderRepository.GetByDriverIdAsync(driverId));

        public async Task<List<OrderDto>> GetAllOrdersAsync()
            => await MapListAsync(await _orderRepository.GetAllAsync());

        // ──────────────────────────────────────────────────────────────────
        // CANCEL
        // ──────────────────────────────────────────────────────────────────
        public async Task<bool> CancelOrderAsync(int orderId, int customerId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)         throw new NotFoundException("Order not found.");
            if (order.CustomerId != customerId) throw new ForbiddenException("You can only cancel your own orders.");
            if (order.OrderStatus != "Pending") throw new BadRequestException("Only pending orders can be cancelled.");

            var ok = await _orderRepository.CancelAsync(orderId);
            if (ok)
            {
                var num = _orderMeta.TryGetValue(orderId, out var m) ? m.OrderNumber : $"#{orderId}";
                _ = SendNotification(customerId, "Order Cancelled", $"Your order {num} has been cancelled.", "Order");
            }
            return ok;
        }

        // ──────────────────────────────────────────────────────────────────
        // UPDATE STATUS
        // ──────────────────────────────────────────────────────────────────
        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto, int userId, string role)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order not found.");

            if (role != "Admin" && role != "Restaurant")
                throw new ForbiddenException("Only a restaurant or admin can update order status.");

            var valid = new[] { "Pending", "Confirmed", "Preparing", "Ready", "OutForDelivery", "Delivered", "Cancelled" };
            if (!valid.Contains(dto.Status))
                throw new BadRequestException($"Invalid status. Use one of: {string.Join(", ", valid)}");

            var updated = await _orderRepository.UpdateStatusAsync(orderId, dto.Status);
            if (updated == null) throw new NotFoundException("Order not found after update.");

            if (updated.CustomerId.HasValue)
            {
                var num = _orderMeta.TryGetValue(orderId, out var m) ? m.OrderNumber : $"#{orderId}";
                _ = SendNotification(updated.CustomerId.Value, $"Order {dto.Status}",
                    $"Your order {num} is now '{dto.Status}'.", "Order");
            }

            return await BuildDtoAsync(updated);
        }

        // ──────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ──────────────────────────────────────────────────────────────────

        private async Task<List<OrderDto>> MapListAsync(IEnumerable<Order> orders)
        {
            var result = new List<OrderDto>();
            foreach (var o in orders) result.Add(await BuildDtoAsync(o));
            return result;
        }

        private async Task<OrderDto> BuildDtoAsync(Order order)
        {
            var items = await _orderItemRepository.GetByOrderIdAsync(order.OrderId);
            _orderMeta.TryGetValue(order.OrderId, out var meta);

            var itemDtos = items.Select(oi => new OrderItemDto
            {
                MenuItemId = oi.ItemId ?? 0,
                ItemName   = oi.Item?.ItemName ?? "Unknown",
                Quantity   = oi.Quantity ?? 0,
                UnitPrice  = oi.Item?.ItemPrice ?? 0m,
                Subtotal   = (oi.Item?.ItemPrice ?? 0m) * (oi.Quantity ?? 0)
            }).ToList();

            return new OrderDto
            {
                Id             = order.OrderId,
                OrderNumber    = meta?.OrderNumber ?? $"ORD{1000 + order.OrderId}",
                OrderDate      = order.OrderDate ?? DateTime.UtcNow,
                OrderStatus    = order.OrderStatus,
                CustomerId     = order.CustomerId ?? 0,
                CustomerName   = order.Customer?.CustomerName,
                RestaurantId   = order.RestaurantId ?? 0,
                RestaurantName = order.Restaurant?.RestaurantName,
                DeliveryDriverId = order.DeliveryDriverId,
                Subtotal       = meta?.Subtotal ?? 0m,
                TaxAmount      = meta?.TaxAmount ?? 0m,
                DeliveryFee    = meta?.DeliveryFee ?? 0m,
                DiscountAmount = meta?.DiscountAmount ?? 0m,
                TotalAmount    = meta?.TotalAmount ?? 0m,
                CustomerRemarks = meta?.CustomerRemarks,
                CanCancel      = order.OrderStatus == "Pending",
                Items          = itemDtos
            };
        }

        private async Task SendNotification(int userId, string title, string message, string type)
        {
            if (userId == 0) return;
            try
            {
                var client  = _httpClientFactory.CreateClient();
                var payload = $"{{\"userId\":{userId},\"title\":\"{title}\",\"message\":\"{message}\",\"type\":\"{type}\"}}";
                await client.PostAsync("https://localhost:7003/api/notifications",
                    new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));
            }
            catch { /* notification failure must not break the order */ }
        }

        // Supplement for missing DB columns
        private class OrderMeta
        {
            public string? OrderNumber    { get; set; }
            public decimal Subtotal       { get; set; }
            public decimal TaxAmount      { get; set; }
            public decimal DeliveryFee    { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal TotalAmount    { get; set; }
            public string? CustomerRemarks { get; set; }
            public string? PaymentMethod  { get; set; }
        }
    }
}
