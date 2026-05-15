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

public class MenuItemServiceTests
{
    private readonly IMapper _mapper;

    public MenuItemServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMenuItems()
    {
        var mockRepo =
            new Mock<IMenuItemRepository>();

        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<MenuItem>
            {
                new MenuItem
                {
                    ItemId = 1,
                    ItemName = "Burger"
                }
            });

        var service =
            new MenuItemService(
                mockRepo.Object,
                _mapper
            );

        var result =
            await service.GetAllAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMenuItem()
    {
        var mockRepo =
            new Mock<IMenuItemRepository>();

        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(new MenuItem
            {
                ItemId = 1,
                ItemName = "Pizza"
            });

        var service =
            new MenuItemService(
                mockRepo.Object,
                _mapper
            );

        var result =
            await service.GetByIdAsync(1);

        Assert.Equal(
            "Pizza",
            result.ItemName
        );
    }

    [Fact]
    public async Task CreateAsync_CreatesMenuItem()
    {
        var mockRepo =
            new Mock<IMenuItemRepository>();

        var service =
            new MenuItemService(
                mockRepo.Object,
                _mapper
            );

        var dto = new CreateMenuItemDto
        {
            ItemName = "Fries",
            ItemDescription = "Crispy fries",
            ItemPrice = 150,
            RestaurantId = 1
        };

        var result =
            await service.CreateAsync(dto);

        Assert.Equal(
            "Fries",
            result.ItemName
        );
    }

    [Fact]
    public async Task UpdateAsync_UpdatesMenuItem()
    {
        var mockRepo =
            new Mock<IMenuItemRepository>();

        var menuItem = new MenuItem
        {
            ItemId = 1,
            ItemName = "Old Burger"
        };

        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(menuItem);

        var service =
            new MenuItemService(
                mockRepo.Object,
                _mapper
            );

        var dto = new UpdateMenuItemDto
        {
            ItemName = "New Burger"
        };

        var result =
            await service.UpdateAsync(1, dto);

        Assert.Equal(
            "New Burger",
            result.ItemName
        );
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ThrowsException()
    {
        var mockRepo =
            new Mock<IMenuItemRepository>();

        mockRepo.Setup(repo => repo.GetByIdAsync(100))
            .ReturnsAsync((MenuItem?)null);

        var service =
            new MenuItemService(
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
            new Mock<IMenuItemRepository>();

        mockRepo.Setup(repo => repo.GetByIdAsync(100))
            .ReturnsAsync((MenuItem?)null);

        var service =
            new MenuItemService(
                mockRepo.Object,
                _mapper
            );

        var dto = new UpdateMenuItemDto
        {
            ItemName = "Test"
        };

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(100, dto)
        );
    }

    [Fact]
    public async Task GetMenuItemsByRestaurantIdAsync_ReturnsEmpty()
    {
        var mockRepo =
            new Mock<IMenuItemRepository>();

        mockRepo.Setup(repo =>
            repo.GetMenuItemsByRestaurantIdAsync(1))
            .ReturnsAsync(new List<MenuItem>());

        var service =
            new MenuItemService(
                mockRepo.Object,
                _mapper
            );

        var result =
            await service
                .GetMenuItemsByRestaurantIdAsync(1);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList()
    {
    var mockRepo =
        new Mock<IMenuItemRepository>();

    mockRepo.Setup(repo => repo.GetAllAsync())
        .ReturnsAsync(new List<MenuItem>());

    var service =
        new MenuItemService(
            mockRepo.Object,
            _mapper
        );

    var result =
        await service.GetAllAsync();
        Assert.Empty(result);
    }
}