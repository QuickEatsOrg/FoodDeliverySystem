using FoodDelivery.API.Controllers.Sanjana;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Sanjana;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FoodDelivery.Tests.UnitTests.Controllers.Sanjana
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _serviceMock;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _serviceMock = new Mock<ICustomerService>();
            _controller = new CustomerController(_serviceMock.Object);
        }

        //Positive Test Cases

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsOk_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<CustomerResponseDto> { new CustomerResponseDto { CustomerId = 1, CustomerName = "John" } };
            _serviceMock.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllCustomersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CustomerResponseDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(customers, response.Data);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsOk_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var customer = new CustomerResponseDto { CustomerId = customerId, CustomerName = "John" };
            _serviceMock.Setup(s => s.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerByIdAsync(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CustomerResponseDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(customer, response.Data);
        }

        [Fact]
        public async Task CreateCustomerAsync_ReturnsCreated_WithNewCustomer()
        {
            // Arrange
            var dto = new CreateCustomerDto { CustomerName = "John", CustomerEmail = "john@example.com" };
            var responseDto = new CustomerResponseDto { CustomerId = 1, CustomerName = "John" };
            _serviceMock.Setup(s => s.CreateCustomerAsync(dto)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.CreateCustomerAsync(dto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            var response = Assert.IsType<ApiResponse<CustomerResponseDto>>(statusCodeResult.Value);
            Assert.True(response.Success);
            Assert.Equal(responseDto, response.Data);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var customerId = 1;
            var dto = new UpdateCustomerDto { CustomerName = "John Updated" };
            var responseDto = new CustomerResponseDto { CustomerId = customerId, CustomerName = "John Updated" };
            _serviceMock.Setup(s => s.UpdateCustomerAsync(customerId, dto)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.UpdateCustomerAsync(customerId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CustomerResponseDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(responseDto, response.Data);
        }



        //Negative Test Cases

        [Fact]
        public async Task GetCustomerByIdAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 99;
            _serviceMock.Setup(s => s.GetCustomerByIdAsync(customerId)).ThrowsAsync(new NotFoundException("Not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetCustomerByIdAsync(customerId));
        }

        [Fact]
        public async Task CreateCustomerAsync_ThrowsConflictException_WhenEmailAlreadyExists()
        {
            // Arrange
            var dto = new CreateCustomerDto { CustomerEmail = "existing@example.com" };
            _serviceMock.Setup(s => s.CreateCustomerAsync(dto)).ThrowsAsync(new ConflictException("Already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _controller.CreateCustomerAsync(dto));
        }

        [Fact]
        public async Task UpdateCustomerAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 99;
            var dto = new UpdateCustomerDto { CustomerName = "Update" };
            _serviceMock.Setup(s => s.UpdateCustomerAsync(customerId, dto)).ThrowsAsync(new NotFoundException("Not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.UpdateCustomerAsync(customerId, dto));
        }

        [Fact]
        public async Task GetCustomerByEmailAsync_ThrowsNotFoundException_WhenEmailNotFound()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _serviceMock.Setup(s => s.GetCustomerByEmailAync(email)).ThrowsAsync(new NotFoundException("Not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetCustomerByEmailAsync(email));
        }
    }
}
