using AutoMapper;
using FoodDelivery.API.DTOs.Neha;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services.Interfaces.Neha;
using FoodDelivery.API.Exceptions;

namespace FoodDelivery.API.Services.Implementations.Neha
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingService(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RatingDto>> GetAllRatingsAsync()
        {
            var ratings = await _ratingRepository.GetAllRatingsAsync();

            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<RatingDto?> GetByIdAsync(int ratingId)
        {
            var rating = await _ratingRepository.GetByIdAsync(ratingId);

            if (rating == null)
            {
                throw new NotFoundException("Rating not found");
            }

            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<IEnumerable<RatingDto>> GetRestaurantRatingsAsync(int restaurantId)
        {
            var ratings = await _ratingRepository.GetRestaurantRatingsAsync(restaurantId);

            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<double> GetAverageRatingAsync(int restaurantId)
        {
            return await _ratingRepository.GetAverageRatingAsync(restaurantId);
        }

        public async Task<RatingDto?> GetByOrderIdAsync(int orderId)
        {
            var rating = await _ratingRepository.GetByOrderIdAsync(orderId);

            if (rating == null)
            {
                throw new NotFoundException("Rating not found for this order");
            }

            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<string> AddAsync(CreateRatingDto ratingDto)
        {
            var existingRating = await _ratingRepository
                .GetByOrderIdAsync(ratingDto.OrderId ?? 0);

            if (existingRating != null)
            {
                throw new BadRequestException("Rating already exists for this order");
            }

            var rating = _mapper.Map<Rating>(ratingDto);

            rating.RatingId = await _ratingRepository.GetNextRatingIdAsync();

            await _ratingRepository.AddAsync(rating);

            await _ratingRepository.SaveChangesAsync();

            return "Rating added successfully";
        }

        public async Task<string> UpdateAsync(int ratingId, UpdateRatingDto ratingDto)
        {
            var existingRating = await _ratingRepository.GetByIdAsync(ratingId);

            if (existingRating == null)
            {
                throw new NotFoundException("Rating not found");
            }

            existingRating.Rating1 = ratingDto.Rating1;

            existingRating.Review = ratingDto.Review;

            await _ratingRepository.UpdateAsync(existingRating);

            await _ratingRepository.SaveChangesAsync();

            return "Rating updated successfully";
        }
    }
}