using FoodDelivery.API.Controllers;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FoodDelivery.Tests.UnitTests.Controllers.Sahil
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly CartController _cartController;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _cartController  = new CartController(_cartServiceMock.Object);
        }

        // POSITIVE TEST CASES (4)
        [Fact]
        public async Task GetCart_WhenCartExists_ShouldReturnOkWithCartData()
        {
            // Arrange
            var cartId   = "cart-user-1";
            var fakeCart = new CartDto
            {
                CartId         = cartId,
                RestaurantName = "Dominos",
                Items          = new List<CartItemDto>
                {
                    new CartItemDto { MenuItemId = 1, Name = "Pizza", Price = 299m, Quantity = 2, Subtotal = 598m }
                },
                Subtotal    = 598m,
                DeliveryFee = 40m,
                TaxAmount   = 29.9m,
                TotalAmount = 667.9m,
                IsEmpty     = false
            };

            _cartServiceMock
                .Setup(s => s.GetCartAsync(cartId))
                .ReturnsAsync(fakeCart);

            // Act
            var result = await _cartController.GetCart(cartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(cartId, data.CartId);
            Assert.Equal("Dominos", data.RestaurantName);
            Assert.Single(data.Items);
        }

        [Fact]
        public async Task AddToCart_WithValidItem_ShouldReturnOkWithUpdatedCart()
        {
            // Arrange
            var cartId = "cart-user-1";
            var dto    = new AddToCartDto { MenuItemId = 5, Quantity = 2 };
            var updatedCart = new CartDto
            {
                CartId  = cartId,
                Items   = new List<CartItemDto> { new CartItemDto { MenuItemId = 5, Name = "Burger", Quantity = 2 } },
                IsEmpty = false
            };

            _cartServiceMock
                .Setup(s => s.AddToCartAsync(cartId, dto.MenuItemId, dto.Quantity))
                .ReturnsAsync(updatedCart);

            // Act
            var result = await _cartController.AddToCart(cartId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsType<CartDto>(okResult.Value);
            Assert.Single(data.Items);
            Assert.Equal(5, data.Items[0].MenuItemId);
        }

        [Fact]
        public async Task RemoveFromCart_WhenItemExists_ShouldReturnOkWithCartWithoutItem()
        {
            // Arrange
            var cartId     = "cart-user-1";
            var menuItemId = 5;
            var updatedCart = new CartDto
            {
                CartId  = cartId,
                Items   = new List<CartItemDto>(),
                IsEmpty = true
            };

            _cartServiceMock
                .Setup(s => s.RemoveFromCartAsync(cartId, menuItemId))
                .ReturnsAsync(updatedCart);

            // Act
            var result = await _cartController.RemoveFromCart(cartId, menuItemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data     = Assert.IsType<CartDto>(okResult.Value);
            Assert.Empty(data.Items);
            Assert.True(data.IsEmpty);
        }

        [Fact]
        public async Task ClearCart_WhenCalled_ShouldReturnOkWithSuccessMessage()
        {
            // Arrange
            var cartId = "cart-user-1";

            _cartServiceMock
                .Setup(s => s.ClearCartAsync(cartId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _cartController.ClearCart(cartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _cartServiceMock.Verify(s => s.ClearCartAsync(cartId), Times.Once);
        }

        // NEGATIVE TEST CASES (4)
        [Fact]
        public async Task GetCart_WhenServiceThrowsException_ShouldReturn500()
        {
            // Arrange
            var cartId = "invalid-cart";

            _cartServiceMock
                .Setup(s => s.GetCartAsync(cartId))
                .ThrowsAsync(new Exception("Session error"));

            // Act
            var result = await _cartController.GetCart(cartId);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task AddToCart_WhenServiceThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            var cartId = "cart-user-1";
            var dto    = new AddToCartDto { MenuItemId = 999, Quantity = 1 };

            _cartServiceMock
                .Setup(s => s.AddToCartAsync(cartId, dto.MenuItemId, dto.Quantity))
                .ThrowsAsync(new Exception("Menu item not found."));

            // Act
            var result = await _cartController.AddToCart(cartId, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCartItem_WhenServiceThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            var cartId     = "cart-user-1";
            var menuItemId = 5;
            var dto        = new UpdateCartItemDto { Quantity = 0 };

            _cartServiceMock
                .Setup(s => s.UpdateCartItemAsync(cartId, menuItemId, dto.Quantity))
                .ThrowsAsync(new Exception("Quantity cannot be zero."));

            // Act
            var result = await _cartController.UpdateCartItem(cartId, menuItemId, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ApplyCoupon_WithEmptyCouponCode_ShouldReturnBadRequest()
        {
            // Arrange
            var cartId = "cart-user-1";
            var dto    = new ApplyCouponDto { CouponCode = "" };  // empty code

            // Act
            var result = await _cartController.ApplyCoupon(cartId, dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequest.Value);
            // Service should NEVER be called with empty code
            _cartServiceMock.Verify(s => s.ApplyCouponAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
        }
    }
}
