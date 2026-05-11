using FoodDelivery.API.Controllers.Tushar;
using FoodDelivery.API.DTOs.Tushar;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Tushar;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FoodDelivery.Tests.UnitTests.Controllers
{
    public class DeliveryControllerTest
    {
        private readonly Mock<IDeliveryService> _deliveryServiceMock;
        private readonly DeliveryController _deliveryController;

        public DeliveryControllerTest()
        {
            _deliveryServiceMock = new Mock<IDeliveryService>();
            _deliveryController = new DeliveryController(_deliveryServiceMock.Object);
        }

        // ---------------- POSITIVE TEST CASES ----------------

        [Fact]
        public async Task GetAllDeliveries_ShouldReturnOkResult()
        {
            var deliveries = new List<DeliveryDto>
            {
                new DeliveryDto
                {
                    OrderId = 1,
                    DeliveryDriverId = 1,
                    DriverName = "Rahul",
                    DriverPhone = "9876543210",
                    DriverVehicle = "Bike",
                    DriverEmail = "rahul@driver.com",
                    OrderStatus = "Assigned"
                }
            };

            _deliveryServiceMock
                .Setup(service => service.GetAllDeliveriesAsync())
                .ReturnsAsync(deliveries);

            var result = await _deliveryController.GetAllDeliveries();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<DeliveryDto>>(okResult.Value);

            Assert.Single(data);
        }

        [Fact]
        public async Task GetDeliveryByOrderId_WhenOrderExists_ShouldReturnOkResult()
        {
            var delivery = new DeliveryDto
            {
                OrderId = 1,
                DeliveryDriverId = 1,
                DriverName = "Rahul",
                DriverPhone = "9876543210",
                DriverVehicle = "Bike",
                DriverEmail = "rahul@driver.com",
                OrderStatus = "Assigned"
            };

            _deliveryServiceMock
                .Setup(service => service.GetDeliveryByOrderIdAsync(1))
                .ReturnsAsync(delivery);

            var result = await _deliveryController.GetDeliveryByOrderId(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<DeliveryDto>(okResult.Value);

            Assert.Equal(1, data.OrderId);
            Assert.Equal("Assigned", data.OrderStatus);
        }

        [Fact]
        public async Task AssignDriver_WhenValidData_ShouldReturnOkResult()
        {
            var assignDto = new AssignDriverDto
            {
                OrderId = 1,
                DriverId = 1
            };

            var delivery = new DeliveryDto
            {
                OrderId = 1,
                DeliveryDriverId = 1,
                DriverName = "Rahul",
                DriverPhone = "9876543210",
                DriverVehicle = "Bike",
                DriverEmail = "rahul@driver.com",
                OrderStatus = "Assigned"
            };

            _deliveryServiceMock
                .Setup(service => service.AssignDriverAsync(assignDto))
                .ReturnsAsync(delivery);

            var result = await _deliveryController.AssignDriver(assignDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<DeliveryDto>(okResult.Value);

            Assert.Equal(1, data.OrderId);
            Assert.Equal(1, data.DeliveryDriverId);
            Assert.Equal("Assigned", data.OrderStatus);
        }

        [Fact]
        public async Task UpdateDeliveryStatus_WhenOrderExists_ShouldReturnOkResult()
        {
            var updateDto = new UpdateDeliveryStatusDto
            {
                OrderStatus = "Delivered"
            };

            var delivery = new DeliveryDto
            {
                OrderId = 1,
                DeliveryDriverId = 1,
                DriverName = "Rahul",
                OrderStatus = "Delivered"
            };

            _deliveryServiceMock
                .Setup(service => service.UpdateDeliveryStatusAsync(1, updateDto))
                .ReturnsAsync(delivery);

            var result = await _deliveryController.UpdateDeliveryStatus(1, updateDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<DeliveryDto>(okResult.Value);

            Assert.Equal(1, data.OrderId);
            Assert.Equal("Delivered", data.OrderStatus);
        }

        // ---------------- NEGATIVE TEST CASES ----------------

        [Fact]
        public async Task GetDeliveryByOrderId_WhenOrderDoesNotExist_ShouldReturnNotFound()
        {
            _deliveryServiceMock
                .Setup(service => service.GetDeliveryByOrderIdAsync(99))
                .ThrowsAsync(new NotFoundException("Delivery/order not found"));

            var result = await _deliveryController.GetDeliveryByOrderId(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Delivery/order not found", notFoundResult.Value);
        }

        [Fact]
        public async Task AssignDriver_WhenOrderOrDriverDoesNotExist_ShouldReturnNotFound()
        {
            var assignDto = new AssignDriverDto
            {
                OrderId = 99,
                DriverId = 99
            };

            _deliveryServiceMock
                .Setup(service => service.AssignDriverAsync(assignDto))
                .ThrowsAsync(new NotFoundException("Order or driver not found"));

            var result = await _deliveryController.AssignDriver(assignDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Order or driver not found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateDeliveryStatus_WhenOrderDoesNotExist_ShouldReturnNotFound()
        {
            var updateDto = new UpdateDeliveryStatusDto
            {
                OrderStatus = "Delivered"
            };

            _deliveryServiceMock
                .Setup(service => service.UpdateDeliveryStatusAsync(99, updateDto))
                .ThrowsAsync(new NotFoundException("Delivery/order not found"));

            var result = await _deliveryController.UpdateDeliveryStatus(99, updateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Delivery/order not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetDeliveriesByDriverId_WhenServiceThrowsException_ShouldThrowException()
        {
            _deliveryServiceMock
                .Setup(service => service.GetDeliveriesByDriverIdAsync(99))
                .ThrowsAsync(new NotFoundException("Driver deliveries not found"));

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _deliveryController.GetDeliveriesByDriverId(99));
        }
    }
}