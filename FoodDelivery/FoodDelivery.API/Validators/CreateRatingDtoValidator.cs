using FluentValidation;
using FoodDelivery.API.DTOs.Neha;

namespace FoodDelivery.API.Validations.Neha
{
    public class CreateRatingDtoValidator : AbstractValidator<CreateRatingDto>
    {
        public CreateRatingDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .NotNull()
                .WithMessage("Order id is required");

            RuleFor(x => x.Rating1)
                .NotNull()
                .WithMessage("Rating is required")
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.Review)
                .MaximumLength(500)
                .WithMessage("Review cannot exceed 500 characters");
        }
    }
}