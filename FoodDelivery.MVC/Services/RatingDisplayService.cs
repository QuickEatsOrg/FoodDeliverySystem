using FoodDelivery.MVC.DTOs;

namespace FoodDelivery.MVC.Services
{
    public class RatingDisplayService : ApiClientBase
    {
        public RatingDisplayService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
        }

        public async Task<(double? AverageRating, int RatingCount)> GetRestaurantRatingAsync(int restaurantId)
        {
            try
            {
                var ratings = await HttpClient.GetFromJsonAsync<List<RatingDto>>($"api/Rating/restaurant/{restaurantId}");
                if (ratings == null || ratings.Count == 0)
                {
                    return (null, 0);
                }

                var scoredRatings = ratings
                    .Where(rating => rating.Rating1.HasValue)
                    .Select(rating => rating.Rating1!.Value)
                    .ToList();

                return scoredRatings.Count == 0
                    ? (null, ratings.Count)
                    : (scoredRatings.Average(), ratings.Count);
            }
            catch (HttpRequestException)
            {
                // TODO: Replace/complete rating display when Rating module is finalized.
                return (null, 0);
            }
        }

        public async Task<Dictionary<int, (double? AverageRating, int RatingCount)>> GetRestaurantRatingsAsync(IEnumerable<int> restaurantIds)
        {
            var ratings = new Dictionary<int, (double? AverageRating, int RatingCount)>();

            foreach (var restaurantId in restaurantIds.Distinct())
            {
                ratings[restaurantId] = await GetRestaurantRatingAsync(restaurantId);
            }

            return ratings;
        }
    }
}
