using FluentValidation;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Validators.Restaurant;

public class CreateRestaurantDtoValidator
    : AbstractValidator<CreateRestaurantDto>
{
    public CreateRestaurantDtoValidator()
    {
        RuleFor(x => x.RestaurantName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Restaurant name is required.");

        RuleFor(x => x.RestaurantAddress)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Restaurant address is required.");

        RuleFor(x => x.RestaurantPhone)
            .NotEmpty()
            .Matches(@"^[0-9]{10}$")
            .WithMessage("Phone number must contain exactly 10 digits.");

        RuleFor(x => x.RestaurantEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&]).+$")
            .WithMessage(
                "Password must contain at least one letter, one number, and one special character."
            );
    }
}