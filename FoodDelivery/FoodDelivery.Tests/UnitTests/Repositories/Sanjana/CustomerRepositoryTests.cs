////using FoodDelivery.API.Data;
////using FoodDelivery.API.Models;
////using FoodDelivery.API.Repositories.Implementations.Sanjana;
////using Microsoft.EntityFrameworkCore;
////using Xunit;

////namespace FoodDelivery.Tests.UnitTests.Repositories.Sanjana
////{
////    public class CustomerRepositoryTests
////    {
////        private FoodDeliveryDbContext GetDbContext()
////        {
////            var options = new DbContextOptionsBuilder<FoodDeliveryDbContext>()
////                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
////                .Options;
////            var context = new FoodDeliveryDbContext(options);
////            context.Database.EnsureCreated();
////            return context;
////        }

////        #region Positive Test Cases

////        [Fact]
////        public async Task CreateCustomerAsync_AddsCustomerToDatabase()
////        {
////            // Arrange
////            using var context = GetDbContext();
////            var repository = new CustomerRepository(context);
////            var customer = new Customer { CustomerName = "John", CustomerEmail = "john@test.com" };

////            // Act
////            var result = await repository.CreateCustomerAsync(customer);

////            // Assert
////            Assert.Equal(1, await context.Customers.CountAsync());
////            Assert.Equal("John", result.CustomerName);
////        }

////        [Fact]
////        public async Task GetAllCustomersAsync_ReturnsAllCustomersWithAddresses()
////        {
////            // Arrange
////            using var context = GetDbContext();
////            var repository = new CustomerRepository(context);
////            context.Customers.Add(new Customer 
////            { 
////                CustomerName = "John", 
////                DeliveryAddresses = new List<DeliveryAddress> { new DeliveryAddress { AddressLine1 = "Addr 1" } } 
////            });
////            await context.SaveChangesAsync();

////            // Act
////            var result = await repository.GetAllCustomersAsync();

////            // Assert
////            Assert.Single(result);
////            Assert.Single(result.First().DeliveryAddresses);
////        }

////        [Fact]
////        public async Task GetCustomerByIdAsync_ReturnsCorrectCustomer()
////        {
////            // Arrange
////            using var context = GetDbContext();
////            var repository = new CustomerRepository(context);
////            var customer = new Customer { CustomerName = "John" };
////            context.Customers.Add(customer);
////            await context.SaveChangesAsync();

////            // Act
////            var result = await repository.GetCustomerByIdAsync(customer.CustomerId);

////            // Assert
////            Assert.NotNull(result);
////            Assert.Equal("John", result.CustomerName);
////        }

////        [Fact]
////        public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
////        {
////            // Arrange
////            using var context = GetDbContext();
////            var repository = new CustomerRepository(context);
////            context.Customers.Add(new Customer { CustomerEmail = "john@test.com" });
////            await context.SaveChangesAsync();

////            // Act
////            var result = await repository.EmailExistsAsync("john@test.com", null);

////            // Assert
////            Assert.True(result);
////        }

////        #endregion

//        #region Negative Test Cases

//        [Fact]
//        public async Task GetCustomerByIdAsync_ReturnsNull_WhenIdDoesNotExist()
//        {
//            // Arrange
//            using var context = GetDbContext();
//            var repository = new CustomerRepository(context);

//            // Act
//            var result = await repository.GetCustomerByIdAsync(999);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task EmailExistsAsync_ReturnsFalse_WhenEmailDoesNotExist()
//        {
//            // Arrange
//            using var context = GetDbContext();
//            var repository = new CustomerRepository(context);

//            // Act
//            var result = await repository.EmailExistsAsync("nonexistent@test.com", null);

//            // Assert
//            Assert.False(result);
//        }

//        [Fact]
//        public async Task PhoneNumberExistsAsync_ReturnsFalse_WhenPhoneDoesNotExist()
//        {
//            // Arrange
//            using var context = GetDbContext();
//            var repository = new CustomerRepository(context);

//            // Act
//            var result = await repository.PhoneNumberExistsAsync("0000000000", null);

//            // Assert
//            Assert.False(result);
//        }

//        [Fact]
//        public async Task EmailExistsAsync_ReturnsFalse_WhenEmailExistsButIsExcludingId()
//        {
//            // Arrange
//            using var context = GetDbContext();
//            var repository = new CustomerRepository(context);
//            var customer = new Customer { CustomerEmail = "john@test.com" };
//            context.Customers.Add(customer);
//            await context.SaveChangesAsync();

//            // Act
//            var result = await repository.EmailExistsAsync("john@test.com", customer.CustomerId);

//            // Assert
//            Assert.False(result);
//        }

//        #endregion
//    }
//}
