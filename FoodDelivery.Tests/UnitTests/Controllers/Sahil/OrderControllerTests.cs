using FoodDelivery.API.Controllers;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FoodDelivery.Tests.UnitTests.Controllers.Sahil
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderController    _orderController;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderController  = new OrderController(_orderServiceMock.Object);
        }

        // POSITIVE TEST CASES (4)
        [Fact]
        public async Task GetAllOrders_WhenOrdersExist_ShouldReturnOkWithList()
        {
            // Arrange
            var orders = new List<OrderDto>
            {
                new OrderDto { Id = 1, OrderNumber = "ORD1001", OrderStatus = "Pending",  CustomerId = 1, RestaurantName = "Dominos" },
                new OrderDto { Id = 2, OrderNumber = "ORD1002", OrderStatus = "Delivered", CustomerId = 2, RestaurantName = "KFC"     }
            };

            _orderServiceMock
                .Setup(s => s.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderController.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(okResult.Value);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task GetOrderById_WhenOrderExists_ShouldReturnOkWithOrder()
        {
            // Arrange
            var order = new OrderDto
            {
                Id             = 1,
                OrderNumber    = "ORD1001",
                OrderStatus    = "Pending",
                CustomerId     = 1,
                CustomerName   = "Sahil",
                RestaurantName = "Dominos",
                TotalAmount    = 500m
            };

            _orderServiceMock
                .Setup(s => s.GetOrderByIdAsync(1, 1, "Customer"))
                .ReturnsAsync(order);

            // Act
            var result = await _orderController.GetOrderById(1, userId: 1, role: "Customer");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(1, data.Id);
            Assert.Equal("ORD1001", data.OrderNumber);
            Assert.Equal("Sahil", data.CustomerName);
        }

        [Fact]
        public async Task GetMyOrders_WhenCustomerHasOrders_ShouldReturnOkWithOrderList()
        {
            // Arrange
            var customerId = 1;
            var myOrders   = new List<OrderDto>
            {
                new OrderDto { Id = 1, OrderNumber = "ORD1001", CustomerId = customerId, OrderStatus = "Delivered" },
                new OrderDto { Id = 3, OrderNumber = "ORD1003", CustomerId = customerId, OrderStatus = "Pending"   }
            };

            _orderServiceMock
                .Setup(s => s.GetMyOrdersAsync(customerId))
                .ReturnsAsync(myOrders);

            // Act
            var result = await _orderController.GetMyOrders(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(okResult.Value);
            Assert.Equal(2, data.Count());
            Assert.All(data, o => Assert.Equal(customerId, o.CustomerId));
        }

        [Fact]
        public async Task CancelOrder_WhenOrderIsPending_ShouldReturnOkWithSuccessMessage()
        {
            // Arrange
            var orderId    = 1;
            var customerId = 1;

            _orderServiceMock
                .Setup(s => s.CancelOrderAsync(orderId, customerId))
                .ReturnsAsync(true);

            // Act
            var result = await _orderController.CancelOrder(orderId, customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _orderServiceMock.Verify(s => s.CancelOrderAsync(orderId, customerId), Times.Once);
        }


        // NEGATIVE TEST CASES (4)
        [Fact]
        public async Task GetOrderById_WhenOrderNotFound_ShouldReturnNotFound()
        {
            // Arrange
            _orderServiceMock
                .Setup(s => s.GetOrderByIdAsync(999, 1, "Customer"))
                .ThrowsAsync(new NotFoundException("Order not found."));

            // Act
            var result = await _orderController.GetOrderById(999, userId: 1, role: "Customer");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderById_WhenUnauthorizedCustomer_ShouldReturn403()
        {
            // Arrange – customer 2 trying to view customer 1's order
            _orderServiceMock
                .Setup(s => s.GetOrderByIdAsync(1, 2, "Customer"))
                .ThrowsAsync(new ForbiddenException("You do not have permission to view this order."));

            // Act
            var result = await _orderController.GetOrderById(1, userId: 2, role: "Customer");

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, statusResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_WithMissingCustomerId_ShouldReturnBadRequest()
        {
            // Arrange
            var dto = new CreateOrderDto
            {
                DeliveryAddressId = 1,
                PaymentMethod     = "Card"
            };

            // Act – customerId = 0 (invalid)
            var result = await _orderController.CreateOrder(dto, customerId: 0, cartId: "cart-1");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrder_WhenOrderAlreadyConfirmed_ShouldReturnBadRequest()
        {
            // Arrange – service rejects cancellation since order is not Pending
            _orderServiceMock
                .Setup(s => s.CancelOrderAsync(1, 1))
                .ThrowsAsync(new BadRequestException("Only pending orders can be cancelled."));

            // Act
            var result = await _orderController.CancelOrder(1, 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
