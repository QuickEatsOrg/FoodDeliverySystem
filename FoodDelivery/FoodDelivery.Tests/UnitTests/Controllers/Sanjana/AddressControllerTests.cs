using FoodDelivery.API.Controllers.Sanjana;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Sanjana;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FoodDelivery.Tests.UnitTests.Controllers.Sanjana
{
    public class AddressControllerTests
    {
        private readonly Mock<IAddressService> _serviceMock;
        private readonly AddressController _controller;

        public AddressControllerTests()
        {
            _serviceMock = new Mock<IAddressService>();
            _controller = new AddressController(_serviceMock.Object);
        }

        [Fact]
        public async Task CreateAddressAsync_ReturnsCreated_WhenSuccessful()
        {
            // Arrange
            var customerId = 1;
            var dto = new CreateAddressDto { AddressLine1 = "123 Main St" };
            var responseDto = new AddressResponseDto { AddressId = 1, AddressLine1 = "123 Main St" };
            _serviceMock.Setup(s => s.CreateAddressAsync(customerId, dto)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.CreateAddressAsync(customerId, dto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            var response = Assert.IsType<ApiResponse<AddressResponseDto>>(statusCodeResult.Value);
            Assert.True(response.Success);
            Assert.Equal(responseDto, response.Data);
        }

        [Fact]
        public async Task GetAddressByAddressIdAsync_ReturnsOk_WhenAddressExists()
        {
            // Arrange
            var addressId = 1;
            var address = new AddressResponseDto { AddressId = addressId, AddressLine1 = "123 Main St" };
            _serviceMock.Setup(s => s.GetAddressByAddressIdAsync(addressId)).ReturnsAsync(address);

            // Act
            var result = await _controller.GetAddressByAddressIdAsync(addressId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<AddressResponseDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(address, response.Data);
        }

        [Fact]
        public async Task GetAddressesByCustomerIdAsync_ReturnsOk_WithListOfAddresses()
        {
            // Arrange
            var customerId = 1;
            var addresses = new List<AddressResponseDto> { new AddressResponseDto { AddressId = 1, AddressLine1 = "123 Main St" } };
            _serviceMock.Setup(s => s.GetAllAddressesByCustomerIdAsync(customerId)).ReturnsAsync(addresses);

            // Act
            var result = await _controller.GetAddressesByCustomerIdAsync(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<AddressResponseDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(addresses, response.Data);
        }

        [Fact]
        public async Task UpdateAddressAsync_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var addressId = 1;
            var dto = new UpdateAddressDto { AddressLine1 = "456 Oak St" };
            var responseDto = new AddressResponseDto { AddressId = addressId, AddressLine1 = "456 Oak St" };
            _serviceMock.Setup(s => s.UpdateAddressAsync(addressId, dto)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.UpdateAddressAsync(addressId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<AddressResponseDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(responseDto, response.Data);
        }

        //Negative Test Cases

        [Fact]
        public async Task CreateAddressAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 99;
            var dto = new CreateAddressDto { AddressLine1 = "123 Main St" };
            _serviceMock.Setup(s => s.CreateAddressAsync(customerId, dto)).ThrowsAsync(new NotFoundException("Customer not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.CreateAddressAsync(customerId, dto));
        }

        [Fact]
        public async Task GetAddressByAddressIdAsync_ThrowsNotFoundException_WhenAddressDoesNotExist()
        {
            // Arrange
            var addressId = 99;
            _serviceMock.Setup(s => s.GetAddressByAddressIdAsync(addressId)).ThrowsAsync(new NotFoundException("Address not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetAddressByAddressIdAsync(addressId));
        }

        [Fact]
        public async Task GetAddressesByCustomerIdAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 99;
            _serviceMock.Setup(s => s.GetAllAddressesByCustomerIdAsync(customerId)).ThrowsAsync(new NotFoundException("Customer not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetAddressesByCustomerIdAsync(customerId));
        }

        [Fact]
        public async Task UpdateAddressAsync_ThrowsNotFoundException_WhenAddressDoesNotExist()
        {
            // Arrange
            var addressId = 99;
            var dto = new UpdateAddressDto { AddressLine1 = "Update" };
            _serviceMock.Setup(s => s.UpdateAddressAsync(addressId, dto)).ThrowsAsync(new NotFoundException("Address not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.UpdateAddressAsync(addressId, dto));
        }
    }
}
