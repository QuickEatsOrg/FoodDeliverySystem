using FoodDelivery.API.DTOs.Neha;
using FoodDelivery.API.Services.Interfaces.Neha;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers.Neha
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // url: api/rating
        [HttpGet]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingService.GetAllRatingsAsync();

            return Ok(ratings);
        }

        // url: api/rating/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            var rating = await _ratingService.GetByIdAsync(id);

            return Ok(rating);
        }

        // url: api/rating/restaurant/1
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetRestaurantRatings(int restaurantId)
        {
            var ratings = await _ratingService.GetRestaurantRatingsAsync(restaurantId);

            return Ok(ratings);
        }

        // url: api/rating/restaurant/1/average
        [HttpGet("restaurant/{restaurantId}/average")]
        public async Task<IActionResult> GetAverageRating(int restaurantId)
        {
            var average = await _ratingService.GetAverageRatingAsync(restaurantId);

            return Ok(average);
        }

        // url: api/rating/order/1
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetRatingByOrderId(int orderId)
        {
            var rating = await _ratingService.GetByOrderIdAsync(orderId);

            return Ok(rating);
        }

        // url: api/rating
        [HttpPost]
        public async Task<IActionResult> AddRating(CreateRatingDto ratingDto)
        {
            var result = await _ratingService.AddAsync(ratingDto);

            return Ok(result);
        }

        // url: api/rating/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(int id, UpdateRatingDto ratingDto)
        {
            var result = await _ratingService.UpdateAsync(id, ratingDto);

            return Ok(result);
        }
    }
}