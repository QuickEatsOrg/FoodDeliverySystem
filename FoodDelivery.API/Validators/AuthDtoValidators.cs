using FluentValidation;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => new[] { "admin", "customer", "deliverydriver", "restaurant" }.Contains(role.ToLower()))
                .WithMessage("Invalid role selected.");
        }
    }

    public class RegisterDriverDtoValidator : AbstractValidator<RegisterDriverDto>
    {
        public RegisterDriverDtoValidator()
        {
            RuleFor(x => x.DriverName)
                .NotEmpty().WithMessage("Driver name is required.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Driver name must contain alphabets only.")
                .MaximumLength(100);

            RuleFor(x => x.DriverPhone)
                .NotEmpty().WithMessage("Driver phone is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits.");

            RuleFor(x => x.DriverVehicle)
                .NotEmpty().WithMessage("Vehicle info is required.")
                .MaximumLength(100);

            RuleFor(x => x.DriverEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter (e.g. Example@123).")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter (e.g. Example@123).")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit (e.g. Example@123).")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g. Example@123).");
        }
    }

    public class RegisterRestaurantDtoValidator : AbstractValidator<RegisterRestaurantDto>
    {
        public RegisterRestaurantDtoValidator()
        {
            RuleFor(x => x.RestaurantName)
                .NotEmpty().WithMessage("Restaurant name is required.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Restaurant name must contain alphabets only.")
                .MaximumLength(100);

            RuleFor(x => x.RestaurantAddress)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200);

            RuleFor(x => x.RestaurantPhone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits.");

            RuleFor(x => x.RestaurantEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter (e.g. Example@123).")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter (e.g. Example@123).")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit (e.g. Example@123).")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g. Example@123).");
        }
    }
}
