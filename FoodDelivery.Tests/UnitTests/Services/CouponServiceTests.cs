using AutoMapper;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace FoodDelivery.Tests.UnitTests.Services
{
    public class CouponServiceTests
    {
        private readonly Mock<ICouponRepository> _couponRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CouponService _couponService;

        public CouponServiceTests()
        {
            _couponRepositoryMock = new Mock<ICouponRepository>();
            _mapperMock = new Mock<IMapper>();
            _couponService = new CouponService(
                _couponRepositoryMock.Object,
                _mapperMock.Object);
        }

        // POSITIVE TESTS

        [Fact]
        public async Task AddAsync_Should_Add_Coupon_Successfully()
        {
            // Arrange
            var couponDto = new CreateCouponDto
            {
                CouponCode = "SAVE10",
                DiscountAmount = 100,
                ExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5))
            };

            var coupon = new Coupon
            {
                CouponCode = "SAVE10"
            };

            _mapperMock
                .Setup(x => x.Map<Coupon>(couponDto))
                .Returns(coupon);

            _couponRepositoryMock
                .Setup(x => x.GetByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((Coupon?)null);

            _couponRepositoryMock
                .Setup(x => x.GetNextCouponIdAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _couponService.AddAsync(couponDto);

            // Assert
            Assert.Equal("Coupon added successfully", result);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Coupon()
        {
            // Arrange
            var coupon = new Coupon
            {
                CouponId = 1,
                CouponCode = "SAVE10"
            };

            var couponDto = new CouponDtos
            {
                CouponId = 1,
                CouponCode = "SAVE10"
            };

            _couponRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(coupon);

            _mapperMock
                .Setup(x => x.Map<CouponDtos>(coupon))
                .Returns(couponDto);

            // Act
            var result = await _couponService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);

            Assert.Equal("SAVE10", result.CouponCode);
        }

        [Fact]
        public async Task GetAllCouponsAsync_Should_Return_All_Coupons()
        {
            // Arrange
            var coupons = new List<Coupon>
            {
                new Coupon
                {
                    CouponId = 1,
                    CouponCode = "SAVE10"
                }
            };

            var couponDtos = new List<CouponDtos>
            {
                new CouponDtos
                {
                    CouponId = 1,
                    CouponCode = "SAVE10"
                }
            };

            _couponRepositoryMock
                .Setup(x => x.GetAllCouponsAsync())
                .ReturnsAsync(coupons);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<CouponDtos>>(coupons))
                .Returns(couponDtos);

            // Act
            var result = await _couponService.GetAllCouponsAsync();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Coupon_Successfully()
        {
            // Arrange
            var coupon = new Coupon
            {
                CouponId = 1,
                CouponCode = "OLDCODE"
            };

            var updateDto = new UpdateCouponDto
            {
                CouponCode = "NEWCODE",
                DiscountAmount = 50,
                ExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
            };

            _couponRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(coupon);

            // Act
            var result = await _couponService.UpdateAsync(1, updateDto);

            // Assert
            Assert.Equal("Coupon updated successfully", result);
        }

        // NEGATIVE TESTS

        [Fact]
        public async Task AddAsync_Should_Throw_Exception_When_Coupon_Already_Exists()
        {
            // Arrange
            var couponDto = new CreateCouponDto
            {
                CouponCode = "SAVE10"
            };

            var coupon = new Coupon
            {
                CouponCode = "SAVE10"
            };

            _mapperMock
                .Setup(x => x.Map<Coupon>(couponDto))
                .Returns(coupon);

            _couponRepositoryMock
                .Setup(x => x.GetByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new Coupon
                {
                    CouponCode = "SAVE10"
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _couponService.AddAsync(couponDto));

            Assert.Equal(
                "Coupon code already exists",
                exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_Exception_When_NotFound()
        {
            // Arrange
            _couponRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Coupon?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _couponService.GetByIdAsync(1));

            Assert.Equal("Coupon not found", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_Exception_When_Coupon_NotFound()
        {
            // Arrange
            var updateDto = new UpdateCouponDto();

            _couponRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Coupon?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _couponService.UpdateAsync(1, updateDto));

            Assert.Equal("Coupon not found", exception.Message);
        }

        [Fact]
        public async Task ValidateCouponAsync_Should_Throw_Exception_When_Invalid()
        {
            // Arrange
            _couponRepositoryMock
                .Setup(x => x.ValidateCouponAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _couponService.ValidateCouponAsync("INVALID"));

            Assert.Equal(
                "Invalid coupon code",
                exception.Message);
        }
    }
}