using AutoMapper;
using FoodDelivery.API.DTOs.Neha;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services.Implementations.Neha;
using Moq;

namespace FoodDelivery.Tests.UnitTests.Services
{
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _ratingRepositoryMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly RatingService _ratingService;

        public RatingServiceTests()
        {
            _ratingRepositoryMock = new Mock<IRatingRepository>();

            _mapperMock = new Mock<IMapper>();

            _ratingService = new RatingService(
                _ratingRepositoryMock.Object,
                _mapperMock.Object);
        }

        // POSITIVE TESTS

        [Fact]
        public async Task AddAsync_Should_Add_Rating_Successfully()
        {
            // Arrange
            var ratingDto = new CreateRatingDto
            {
                OrderId = 1,
                Rating1 = 5,
                Review = "Excellent"
            };

            var rating = new Rating();

            _mapperMock
                .Setup(x => x.Map<Rating>(ratingDto))
                .Returns(rating);

            _ratingRepositoryMock
                .Setup(x => x.GetByOrderIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Rating)null);

            _ratingRepositoryMock
                .Setup(x => x.GetNextRatingIdAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _ratingService.AddAsync(ratingDto);

            // Assert
            Assert.Equal("Rating added successfully", result);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Rating()
        {
            // Arrange
            var rating = new Rating
            {
                RatingId = 1,
                Rating1 = 5
            };

            var ratingDto = new RatingDto
            {
                RatingId = 1,
                Rating1 = 5
            };

            _ratingRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(rating);

            _mapperMock
                .Setup(x => x.Map<RatingDto>(rating))
                .Returns(ratingDto);

            // Act
            var result = await _ratingService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(5, result.Rating1);
        }

        [Fact]
        public async Task GetAverageRatingAsync_Should_Return_Average()
        {
            // Arrange
            _ratingRepositoryMock
                .Setup(x => x.GetAverageRatingAsync(1))
                .ReturnsAsync(4.5);

            // Act
            var result = await _ratingService.GetAverageRatingAsync(1);

            // Assert
            Assert.Equal(4.5, result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Rating_Successfully()
        {
            // Arrange
            var rating = new Rating
            {
                RatingId = 1
            };

            var updateDto = new UpdateRatingDto
            {
                Rating1 = 4,
                Review = "Good"
            };

            _ratingRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(rating);

            // Act
            var result = await _ratingService.UpdateAsync(1, updateDto);

            // Assert
            Assert.Equal("Rating updated successfully", result);
        }

        // NEGATIVE TESTS

        [Fact]
        public async Task AddAsync_Should_Throw_Exception_When_Rating_Already_Exists()
        {
            // Arrange
            var ratingDto = new CreateRatingDto
            {
                OrderId = 1
            };

            _ratingRepositoryMock
                .Setup(x => x.GetByOrderIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Rating());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _ratingService.AddAsync(ratingDto));

            Assert.Equal(
                "Rating already exists for this order",
                exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_Exception_When_NotFound()
        {
            // Arrange
            _ratingRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Rating)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _ratingService.GetByIdAsync(1));

            Assert.Equal("Rating not found", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_Exception_When_Rating_NotFound()
        {
            // Arrange
            var updateDto = new UpdateRatingDto();

            _ratingRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Rating)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _ratingService.UpdateAsync(1, updateDto));

            Assert.Equal("Rating not found", exception.Message);
        }

        [Fact]
        public async Task GetByOrderIdAsync_Should_Throw_Exception_When_NotFound()
        {
            // Arrange
            _ratingRepositoryMock
                .Setup(x => x.GetByOrderIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Rating)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _ratingService.GetByOrderIdAsync(1));

            Assert.Equal(
                "Rating not found for this order",
                exception.Message);
        }
    }
}