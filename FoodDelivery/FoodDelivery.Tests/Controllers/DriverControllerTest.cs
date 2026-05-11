using FoodDelivery.API.Controllers.Tushar;
using FoodDelivery.API.DTOs.Tushar;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Tushar;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FoodDelivery.Tests.UnitTests.Controllers
{
    public class DriverControllerTest
    {
        private readonly Mock<IDriverService> _driverServiceMock;
        private readonly DriverController _driverController;

        public DriverControllerTest()
        {
            _driverServiceMock = new Mock<IDriverService>();
            _driverController = new DriverController(_driverServiceMock.Object);
        }

        // ---------------- POSITIVE TEST CASES ----------------

        [Fact]
        public async Task GetAllDrivers_ShouldReturnOkResult()
        {
            var drivers = new List<DriverDto>
            {
                new DriverDto
                {
                    DriverId = 1,
                    DriverName = "Rahul",
                    DriverPhone = "9876543210",
                    DriverVehicle = "Bike",
                    DriverEmail = "rahul@driver.com",
                    RoleId = 4
                }
            };

            _driverServiceMock
                .Setup(service => service.GetAllDriversAsync())
                .ReturnsAsync(drivers);

            var result = await _driverController.GetAllDrivers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<DriverDto>>(okResult.Value);

            Assert.Single(data);
        }

        [Fact]
        public async Task GetDriverById_WhenDriverExists_ShouldReturnOkResult()
        {
            var driver = new DriverDto
            {
                DriverId = 1,
                DriverName = "Rahul",
                DriverPhone = "9876543210",
                DriverVehicle = "Bike",
                DriverEmail = "rahul@driver.com",
                RoleId = 4
            };

            _driverServiceMock
                .Setup(service => service.GetDriverByIdAsync(1))
                .ReturnsAsync(driver);

            var result = await _driverController.GetDriverById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<DriverDto>(okResult.Value);

            Assert.Equal(1, data.DriverId);
            Assert.Equal("Rahul", data.DriverName);
        }

        [Fact]
        public async Task CreateDriver_WithValidData_ShouldReturnOkResult()
        {
            var createDto = new CreateDriverDto
            {
                DriverName = "Amit",
                DriverPhone = "9999999999",
                DriverVehicle = "Scooter",
                DriverEmail = "amit@driver.com"
            };

            var createdDriver = new DriverDto
            {
                DriverId = 2,
                DriverName = "Amit",
                DriverPhone = "9999999999",
                DriverVehicle = "Scooter",
                DriverEmail = "amit@driver.com",
                RoleId = 4
            };

            _driverServiceMock
                .Setup(service => service.CreateDriverAsync(createDto))
                .ReturnsAsync(createdDriver);

            var result = await _driverController.CreateDriver(createDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<DriverDto>(okResult.Value);

            Assert.Equal(2, data.DriverId);
            Assert.Equal("Amit", data.DriverName);
        }

        [Fact]
        public async Task UpdateDriver_WhenDriverExists_ShouldReturnOkResult()
        {
            var updateDto = new UpdateDriverDto
            {
                DriverName = "Updated Rahul",
                DriverPhone = "8888888888",
                DriverVehicle = "Car",
                DriverEmail = "updated@driver.com"
            };

            var updatedDriver = new DriverDto
            {
                DriverId = 1,
                DriverName = "Updated Rahul",
                DriverPhone = "8888888888",
                DriverVehicle = "Car",
                DriverEmail = "updated@driver.com",
                RoleId = 4
            };

            _driverServiceMock
                .Setup(service => service.UpdateDriverAsync(1, updateDto))
                .ReturnsAsync(updatedDriver);

            var result = await _driverController.UpdateDriver(1, updateDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<DriverDto>(okResult.Value);

            Assert.Equal(1, data.DriverId);
            Assert.Equal("Updated Rahul", data.DriverName);
        }

        // ---------------- NEGATIVE TEST CASES ----------------

        [Fact]
        public async Task GetDriverById_WhenDriverDoesNotExist_ShouldReturnNotFound()
        {
            _driverServiceMock
                .Setup(service => service.GetDriverByIdAsync(99))
                .ThrowsAsync(new NotFoundException("Driver not found"));

            var result = await _driverController.GetDriverById(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Driver not found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateDriver_WhenDriverDoesNotExist_ShouldReturnNotFound()
        {
            var updateDto = new UpdateDriverDto
            {
                DriverName = "Updated Driver",
                DriverPhone = "8888888888",
                DriverVehicle = "Car",
                DriverEmail = "updated@driver.com"
            };

            _driverServiceMock
                .Setup(service => service.UpdateDriverAsync(99, updateDto))
                .ThrowsAsync(new NotFoundException("Driver not found"));

            var result = await _driverController.UpdateDriver(99, updateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Driver not found", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteDriver_WhenDriverDoesNotExist_ShouldReturnNotFound()
        {
            _driverServiceMock
                .Setup(service => service.DeleteDriverAsync(99))
                .ThrowsAsync(new NotFoundException("Driver not found"));

            var result = await _driverController.DeleteDriver(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Driver not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateDriver_WhenServiceThrowsException_ShouldThrowException()
        {
            var createDto = new CreateDriverDto
            {
                DriverName = "",
                DriverPhone = "",
                DriverVehicle = "",
                DriverEmail = "wrong-email"
            };

            _driverServiceMock
                .Setup(service => service.CreateDriverAsync(createDto))
                .ThrowsAsync(new BadRequestException("Invalid driver data"));

            await Assert.ThrowsAsync<BadRequestException>(() =>
                _driverController.CreateDriver(createDto));
        }
    }
}