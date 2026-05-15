using AutoMapper;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Mappings;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FoodDelivery.Tests.UnitTests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
            
            _service = new CustomerService(_repositoryMock.Object, _mapper);
        }

       //Positive Test Cases

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsMappedDtos_WithOrderCounts()
        {
            // Arrange
            var customers = new List<Customer> 
            { 
                new Customer { CustomerId = 1, CustomerName = "John", DeliveryAddresses = new List<DeliveryAddress>() } 
            };
            _repositoryMock.Setup(r => r.GetAllCustomersAsync()).ReturnsAsync(customers);
            _repositoryMock.Setup(r => r.GetCustomerOrderCountAsync(1)).ReturnsAsync(5);

            // Act
            var result = await _service.GetAllCustomersAsync();

            // Assert
            var list = Assert.IsAssignableFrom<IEnumerable<CustomerResponseDto>>(result);
            var first = list.First();
            Assert.Equal(1, first.CustomerId);
            Assert.Equal(5, first.TotalOrders);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsCorrectDto_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { CustomerId = customerId, CustomerName = "John", DeliveryAddresses = new List<DeliveryAddress>() };
            _repositoryMock.Setup(r => r.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);
            _repositoryMock.Setup(r => r.GetCustomerOrderCountAsync(customerId)).ReturnsAsync(2);

            // Act
            var result = await _service.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal(2, result.TotalOrders);
        }

        [Fact]
        public async Task CreateCustomerAsync_SuccessfullyCreates_WhenValid()
        {
            // Arrange
            var dto = new CreateCustomerDto { CustomerName = "John", CustomerEmail = "john@test.com", CustomerPhone = "1234567890" };
            var customer = new Customer { CustomerId = 1, CustomerName = "John", CustomerEmail = "john@test.com", CustomerPhone = "1234567890", DeliveryAddresses = new List<DeliveryAddress>() };
            
            _repositoryMock.Setup(r => r.EmailExistsAsync(dto.CustomerEmail, null)).ReturnsAsync(false);
            _repositoryMock.Setup(r => r.PhoneNumberExistsAsync(dto.CustomerPhone, null)).ReturnsAsync(false);
            _repositoryMock.Setup(r => r.CreateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(customer);
            _repositoryMock.Setup(r => r.GetCustomerByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _service.CreateCustomerAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.CustomerName);
        }

        [Fact]
        public async Task UpdateCustomerAsync_UpdatesFieldsCorrectly()
        {
            // Arrange
            var customerId = 1;
            var existing = new Customer { CustomerId = customerId, CustomerName = "Old Name", CustomerEmail = "old@test.com", DeliveryAddresses = new List<DeliveryAddress>() };
            var dto = new UpdateCustomerDto { CustomerName = "New Name" };
            
            _repositoryMock.Setup(r => r.GetCustomerByIdAsync(customerId)).ReturnsAsync(existing);
            _repositoryMock.Setup(r => r.UpdateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(existing);

            // Act
            var result = await _service.UpdateCustomerAsync(customerId, dto);

            // Assert
            Assert.Equal("New Name", existing.CustomerName);
            _repositoryMock.Verify(r => r.UpdateCustomerAsync(existing), Times.Once);
        }


        //Negative Test Cases

        [Fact]
        public async Task GetCustomerByIdAsync_ThrowsNotFoundException_WhenRepositoryReturnsNull()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetCustomerByIdAsync(1)).ReturnsAsync((Customer?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetCustomerByIdAsync(1));
        }

        [Fact]
        public async Task CreateCustomerAsync_ThrowsConflictException_WhenEmailExists()
        {
            // Arrange
            var dto = new CreateCustomerDto { CustomerEmail = "exists@test.com" };
            _repositoryMock.Setup(r => r.EmailExistsAsync(dto.CustomerEmail, null)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.CreateCustomerAsync(dto));
        }

        [Fact]
        public async Task CreateCustomerAsync_ThrowsConflictException_WhenPhoneExists()
        {
            // Arrange
            var dto = new CreateCustomerDto { CustomerEmail = "new@test.com", CustomerPhone = "12345" };
            _repositoryMock.Setup(r => r.EmailExistsAsync(dto.CustomerEmail, null)).ReturnsAsync(false);
            _repositoryMock.Setup(r => r.PhoneNumberExistsAsync(dto.CustomerPhone, null)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.CreateCustomerAsync(dto));
        }

        [Fact]
        public async Task UpdateCustomerAsync_ThrowsConflictException_WhenNewEmailAlreadyTakenByAnother()
        {
            // Arrange
            var customerId = 1;
            var existing = new Customer { CustomerId = customerId, CustomerEmail = "old@test.com" };
            var dto = new UpdateCustomerDto { CustomerEmail = "taken@test.com" };
            
            _repositoryMock.Setup(r => r.GetCustomerByIdAsync(customerId)).ReturnsAsync(existing);
            _repositoryMock.Setup(r => r.EmailExistsAsync(dto.CustomerEmail, customerId)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.UpdateCustomerAsync(customerId, dto));
        }

    }
}
