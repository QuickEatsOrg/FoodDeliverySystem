using FoodDelivery.API.Controllers.Sahil;
using FoodDelivery.API.DTOs.Sahil;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Sahil;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FoodDelivery.Tests.UnitTests.Controllers.Sahil
{
    public class PaymentControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly PaymentController    _paymentController;

        public PaymentControllerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _paymentController  = new PaymentController(_paymentServiceMock.Object);
        }

        // POSITIVE TEST CASES (4)
        [Fact]
        public async Task InitiatePayment_WithValidData_ShouldReturnOkWithResponse()
        {
            // Arrange
            var dto = new InitiatePaymentDto { OrderId = 1, PaymentMethod = "Card" };
            var response = new InitiatePaymentResponseDto
            {
                PaymentNumber = "PAY1001",
                Amount        = 499m,
                PaymentStatus = "Success",
                TransactionId = "TXN123456",
                RedirectUrl   = "/order/confirmation/1"
            };

            _paymentServiceMock
                .Setup(s => s.InitiatePaymentAsync(dto, 1))
                .ReturnsAsync(response);

            // Act
            var result = await _paymentController.InitiatePayment(dto, userId: 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsType<InitiatePaymentResponseDto>(okResult.Value);
            Assert.Equal("PAY1001", data.PaymentNumber);
            Assert.Equal("Success", data.PaymentStatus);
            Assert.Equal("TXN123456", data.TransactionId);
        }

        [Fact]
        public async Task GetPaymentStatus_WhenPaymentExists_ShouldReturnOkWithStatus()
        {
            // Arrange
            var statusDto = new PaymentStatusDto
            {
                OrderId       = 1,
                PaymentStatus = "Success",
                Amount        = 499m,
                TransactionId = "TXN123456"
            };

            _paymentServiceMock
                .Setup(s => s.GetPaymentStatusAsync(1, 1, "Customer"))
                .ReturnsAsync(statusDto);

            // Act
            var result = await _paymentController.GetPaymentStatus(1, userId: 1, role: "Customer");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsType<PaymentStatusDto>(okResult.Value);
            Assert.Equal("Success", data.PaymentStatus);
            Assert.Equal(1, data.OrderId);
        }

        [Fact]
        public async Task GetPaymentHistory_WhenUserHasPayments_ShouldReturnOkWithList()
        {
            // Arrange
            var userId   = 1;
            var payments = new List<PaymentDto>
            {
                new PaymentDto { Id = 1, PaymentNumber = "PAY1001", OrderId = 1, Amount = 499m, PaymentStatus = "Success" },
                new PaymentDto { Id = 2, PaymentNumber = "PAY1002", OrderId = 2, Amount = 299m, PaymentStatus = "Pending" }
            };

            _paymentServiceMock
                .Setup(s => s.GetPaymentHistoryAsync(userId))
                .ReturnsAsync(payments);

            // Act
            var result = await _paymentController.GetPaymentHistory(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsAssignableFrom<IEnumerable<PaymentDto>>(okResult.Value);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task GetAllPayments_WhenAdminCalls_ShouldReturnOkWithAllPayments()
        {
            // Arrange
            var allPayments = new List<PaymentDto>
            {
                new PaymentDto { Id = 1, PaymentNumber = "PAY1001", OrderId = 1, PaymentStatus = "Success" },
                new PaymentDto { Id = 2, PaymentNumber = "PAY1002", OrderId = 2, PaymentStatus = "Pending" },
                new PaymentDto { Id = 3, PaymentNumber = "PAY1003", OrderId = 3, PaymentStatus = "Failed"  }
            };

            _paymentServiceMock
                .Setup(s => s.GetAllPaymentsAsync())
                .ReturnsAsync(allPayments);

            // Act
            var result = await _paymentController.GetAllPayments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsAssignableFrom<IEnumerable<PaymentDto>>(okResult.Value);
            Assert.Equal(3, data.Count());
        }


        // NEGATIVE TEST CASES (4)
        [Fact]
        public async Task InitiatePayment_WithZeroUserId_ShouldReturnBadRequest()
        {
            // Arrange
            var dto = new InitiatePaymentDto { OrderId = 1, PaymentMethod = "UPI" };

            // Act – userId = 0 is invalid
            var result = await _paymentController.InitiatePayment(dto, userId: 0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _paymentServiceMock.Verify(s => s.InitiatePaymentAsync(It.IsAny<InitiatePaymentDto>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task InitiatePayment_WhenOrderNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var dto = new InitiatePaymentDto { OrderId = 999, PaymentMethod = "Card" };

            _paymentServiceMock
                .Setup(s => s.InitiatePaymentAsync(dto, 1))
                .ThrowsAsync(new NotFoundException("Order not found."));

            // Act
            var result = await _paymentController.InitiatePayment(dto, userId: 1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPaymentStatus_WhenUnauthorizedUser_ShouldReturn403()
        {
            // Arrange – customer 2 trying to view customer 1's payment
            _paymentServiceMock
                .Setup(s => s.GetPaymentStatusAsync(1, 2, "Customer"))
                .ThrowsAsync(new ForbiddenException("You do not have permission to view this payment."));

            // Act
            var result = await _paymentController.GetPaymentStatus(1, userId: 2, role: "Customer");

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, statusResult.StatusCode);
        }

        [Fact]
        public async Task InitiatePayment_WhenOrderAlreadyPaid_ShouldReturnBadRequest()
        {
            // Arrange – order already has a successful payment
            var dto = new InitiatePaymentDto { OrderId = 1, PaymentMethod = "Card" };

            _paymentServiceMock
                .Setup(s => s.InitiatePaymentAsync(dto, 1))
                .ThrowsAsync(new BadRequestException("This order has already been paid."));

            // Act
            var result = await _paymentController.InitiatePayment(dto, userId: 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
