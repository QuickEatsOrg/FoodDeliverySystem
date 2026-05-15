using FluentValidation;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Validators.Restaurant;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    public CreateRestaurantDtoValidator()
    {
        // Name: required, alphabets and spaces only
        RuleFor(x => x.RestaurantName)
            .NotEmpty().WithMessage("Restaurant name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Restaurant name must contain alphabets only (no numbers or special characters).");

        // Address: required
        RuleFor(x => x.RestaurantAddress)
            .NotEmpty().WithMessage("Restaurant address is required.")
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

        // Phone: required, exactly 10 digits
        RuleFor(x => x.RestaurantPhone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits (e.g. 9876543210).");

        // Email: required and valid format
        RuleFor(x => x.RestaurantEmail)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be in a valid format (e.g. restaurant@example.com).");

        // Password: strong — uppercase, lowercase, digit, special char (e.g. Example@123)
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter (e.g. Example@123).")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter (e.g. Example@123).")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit (e.g. Example@123).")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character like @, #, ! (e.g. Example@123).");
    }
}