using FoodDelivery.API.DTOs.Neha;

namespace FoodDelivery.API.Services.Interfaces.Neha
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingDto>> GetAllRatingsAsync();

        Task<RatingDto?> GetByIdAsync(int ratingId);

        Task<IEnumerable<RatingDto>> GetRestaurantRatingsAsync(int restaurantId);

        Task<double> GetAverageRatingAsync(int restaurantId);

        Task<RatingDto?> GetByOrderIdAsync(int orderId);

        Task<string> AddAsync(CreateRatingDto ratingDto);

        Task<string> UpdateAsync(int ratingId, UpdateRatingDto ratingDto);
    }
}