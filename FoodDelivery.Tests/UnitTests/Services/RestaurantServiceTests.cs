using Xunit;
using Moq;
using AutoMapper;

using FoodDelivery.API.DTOs;
using FoodDelivery.API.Models;
using FoodDelivery.API.Services;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Mappings;

namespace FoodDelivery.Tests.UnitTests.Services;

public class RestaurantServiceTests
{
    private readonly IMapper _mapper;

    public RestaurantServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsRestaurants()
    {
        var mockRepo =
            new Mock<IRestaurantRepository>();

        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Restaurant>
            {
                new Restaurant
                {
                    RestaurantId = 1,
                    RestaurantName = "KFC"
                }
            });

        var service =
            new RestaurantService(
                mockRepo.Object,
                _mapper
            );

        var result =
            await service.GetAllAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsRestaurant()
    {
        var mockRepo =
            new Mock<IRestaurantRepository>();

        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(new Restaurant
            {
                RestaurantId = 1,
                RestaurantName = "Dominos"
            });

        var service =
            new RestaurantService(
                mockRepo.Object,
                _mapper
            );

        var result =
            await service.GetByIdAsync(1);

        Assert.Equal(
            "Dominos",
            result.RestaurantName
        );
    }

    [Fact]
    public async Task CreateAsync_CreatesRestaurant()
    {
        var mockRepo =
            new Mock<IRestaurantRepository>();

        var service =
            new RestaurantService(
                mockRepo.Object,
                _mapper
            );

        var dto = new CreateRestaurantDto
        {
            RestaurantName = "Pizza Hut",
            RestaurantAddress = "Delhi",
            RestaurantPhone = "9999999999",
            RestaurantEmail = "pizza@test.com",
            Password = "Pass@123"
        };

        var result =
            await service.CreateAsync(dto);

        Assert.Equal(
            "Pizza Hut",
            result.RestaurantName
        );
    }

    [Fact]
    public async Task UpdateAsync_UpdatesRestaurant()
    {
        var mockRepo =
            new Mock<IRestaurantRepository>();

        var restaurant = new Restaurant
        {
            RestaurantId = 1,
            RestaurantName = "Old"
        };

        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(restaurant);

        var service =
            new RestaurantService(
                mockRepo.Object,
                _mapper
            );

        var dto = new UpdateRestaurantDto
        {
            RestaurantName = "New"
        };

        var result =
            await service.UpdateAsync(1, dto);

        Assert.Equal(
            "New",
            result.RestaurantName
        );
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ThrowsException()
    {
        var mockRepo =
            new Mock<IRestaurantRepository>();

        mockRepo.Setup(repo => repo.GetByIdAsync(100))
            .ReturnsAsync((Restaurant?)null);

        var service =
            new RestaurantService(
                mockRepo.Object,
                _mapper
            );

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(100)
        );
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_ThrowsException()
    {
        var mockRepo =
            new Mock<IRestaurantRepository>();

        mockRepo.Setup(repo => repo.GetByIdAsync(100))
            .ReturnsAsync((Restaurant?)null);

        var service =
            new RestaurantService(
                mockRepo.Object,
                _mapper
            );

        var dto = new UpdateRestaurantDto
        {
            RestaurantName = "Test"
        };

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(100, dto)
        );
    }

    [Fact]
    public async Task GetByRestaurantIdAsync_ReturnsEmpty()
    {
        var mockRepo =
            new Mock<IRestaurantRepository>();

        mockRepo.Setup(repo =>
            repo.GetByRestaurantIdAsync(1))
            .ReturnsAsync(new List<MenuItem>());

        var service =
            new RestaurantService(
                mockRepo.Object,
                _mapper
            );

        var result =
            await service.GetByRestaurantIdAsync(1);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList()
    {
    var mockRepo =
        new Mock<IRestaurantRepository>();

    mockRepo.Setup(repo => repo.GetAllAsync())
        .ReturnsAsync(new List<Restaurant>());

    var service =
        new RestaurantService(
            mockRepo.Object,
            _mapper
        );

    var result =
        await service.GetAllAsync();
        Assert.Empty(result);
    }
}