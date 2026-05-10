using FoodDelivery.API.Models;

namespace FoodDelivery.API.Repositories
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllRatingsAsync();
        Task<Rating?> GetByIdAsync(int ratingId);
        Task<IEnumerable<Rating>> GetRestaurantRatingsAsync(int restaurantId);
        Task<double> GetAverageRatingAsync(int restaurantId);
        Task<Rating?> GetByOrderIdAsync(int orderId);
        Task<int> GetNextRatingIdAsync();
        Task AddAsync(Rating rating);
        Task UpdateAsync(Rating rating);
        Task SaveChangesAsync();
    }
}