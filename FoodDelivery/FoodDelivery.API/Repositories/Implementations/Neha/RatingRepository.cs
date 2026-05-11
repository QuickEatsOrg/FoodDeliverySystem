using FoodDelivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Repositories.Implementations
{
    public class RatingRepository : IRatingRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public RatingRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsAsync()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<Rating?> GetByIdAsync(int ratingId)
        {
            return await _context.Ratings.FindAsync(ratingId);
        }

        public async Task<IEnumerable<Rating>> GetRestaurantRatingsAsync(int restaurantId)
        {
            return await _context.Ratings
                .Where(r => r.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int restaurantId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.RestaurantId == restaurantId)
                .ToListAsync();

            if (!ratings.Any())
            {
                return 0;
            }

            return ratings.Average(r => r.Rating1 ?? 0);
        }

        public async Task<Rating?> GetByOrderIdAsync(int orderId) //checking rating existing or not for specific order
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.OrderId == orderId);
        }
        public async Task<int> GetNextRatingIdAsync()
        {
            if (!await _context.Ratings.AnyAsync())
            {
                return 1;
            }

            return await _context.Ratings.MaxAsync(r => r.RatingId) + 1;
        }
        public async Task AddAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
        }

        public Task UpdateAsync(Rating rating)
        {
            _context.Ratings.Update(rating);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}