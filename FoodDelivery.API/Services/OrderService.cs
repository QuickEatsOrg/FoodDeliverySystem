 using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodDelivery.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICartService _cartService;
        private readonly IHttpClientFactory _httpClientFactory;

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

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, int customerId, string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart.IsEmpty)
                throw new BadRequestException("Cart is empty. Add items before placing an order.");

            var order = new Order
            {
                CustomerId = customerId,
                RestaurantId = cart.RestaurantId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending"
            };

            var created = await _orderRepository.CreateAsync(order);
            var orderNumber = $"ORD{1000 + created.OrderId}";

            _orderMeta[created.OrderId] = new OrderMeta
            {
                OrderNumber = orderNumber,
                Subtotal = cart.Subtotal,
                TaxAmount = cart.TaxAmount,
                DeliveryFee = cart.DeliveryFee,
                DiscountAmount = cart.DiscountAmount,
                TotalAmount = cart.TotalAmount,
                CustomerRemarks = dto.CustomerRemarks,
                PaymentMethod = dto.PaymentMethod
            };

            var orderItems = cart.Items.Select(i => new OrderItem
            {
                OrderId = created.OrderId,
                ItemId = i.MenuItemId,
                Quantity = i.Quantity
            }).ToList();

            await _orderItemRepository.CreateRangeAsync(orderItems);
            await _cartService.ClearCartAsync(cartId);

            return await BuildDtoAsync(created);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, int userId, string role)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order not found.");

            bool isAdmin = role == "Admin";
            bool isCustomer = order.CustomerId == userId;
            bool isDriver = order.DeliveryDriverId == userId;

            if (!isAdmin && !isCustomer && !isDriver)
                throw new ForbiddenException("You do not have permission to view this order.");

            return await BuildDtoAsync(order);
        }

        public async Task<List<OrderDto>> GetMyOrdersAsync(int customerId)
            => await MapListAsync(await _orderRepository.GetByCustomerIdAsync(customerId));

        public async Task<List<OrderDto>> GetRestaurantOrdersAsync(int restaurantId)
            => await MapListAsync(await _orderRepository.GetByRestaurantIdAsync(restaurantId));

        public async Task<List<OrderDto>> GetDriverOrdersAsync(int driverId)
            => await MapListAsync(await _orderRepository.GetByDriverIdAsync(driverId));

        public async Task<List<OrderDto>> GetAllOrdersAsync()
            => await MapListAsync(await _orderRepository.GetAllAsync());

        public async Task<bool> CancelOrderAsync(int orderId, int customerId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order not found.");
            if (order.CustomerId != customerId) throw new ForbiddenException("You can only cancel your own orders.");
            if (order.OrderStatus != "Pending") throw new BadRequestException("Only pending orders can be cancelled.");

            return await _orderRepository.CancelAsync(orderId);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto, int userId, string role)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order not found.");

            if (role != "Admin" && role != "Restaurant")
                throw new ForbiddenException("Only a restaurant or admin can update order status.");

            var updated = await _orderRepository.UpdateStatusAsync(orderId, dto.Status);
            if (updated == null) throw new NotFoundException("Order not found after update.");

            return await BuildDtoAsync(updated);
        }

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
                ItemName = oi.Item?.ItemName ?? "Unknown",
                Quantity = oi.Quantity ?? 0,
                UnitPrice = oi.Item?.ItemPrice ?? 0m,
                Subtotal = (oi.Item?.ItemPrice ?? 0m) * (oi.Quantity ?? 0)
            }).ToList();

            return new OrderDto
            {
                Id = order.OrderId,
                OrderNumber = meta?.OrderNumber ?? $"ORD{1000 + order.OrderId}",
                OrderDate = order.OrderDate ?? DateTime.UtcNow,
                OrderStatus = order.OrderStatus,
                CustomerId = order.CustomerId ?? 0,
                CustomerName = order.Customer?.CustomerName,
                RestaurantId = order.RestaurantId ?? 0,
                RestaurantName = order.Restaurant?.RestaurantName,
                DeliveryDriverId = order.DeliveryDriverId,
                Subtotal = meta?.Subtotal ?? 0m,
                TaxAmount = meta?.TaxAmount ?? 0m,
                DeliveryFee = meta?.DeliveryFee ?? 0m,
                DiscountAmount = meta?.DiscountAmount ?? 0m,
                TotalAmount = meta?.TotalAmount ?? 0m,
                CustomerRemarks = meta?.CustomerRemarks,
                CanCancel = order.OrderStatus == "Pending",
                Items = itemDtos
            };
        }

        private class OrderMeta
        {
            public string? OrderNumber { get; set; }
            public decimal Subtotal { get; set; }
            public decimal TaxAmount { get; set; }
            public decimal DeliveryFee { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal TotalAmount { get; set; }
            public string? CustomerRemarks { get; set; }
            public string? PaymentMethod { get; set; }
        }
    }
}
