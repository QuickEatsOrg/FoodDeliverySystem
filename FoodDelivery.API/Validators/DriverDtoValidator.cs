using FluentValidation;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Validators
{
    public class CreateDriverDtoValidator : AbstractValidator<CreateDriverDto>
    {
        public CreateDriverDtoValidator()
        {
            // Name: required, alphabets and spaces only
            RuleFor(x => x.DriverName)
                .NotEmpty().WithMessage("Driver name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Driver name must contain alphabets only (no numbers or special characters).");

            // Phone: required, exactly 10 digits
            RuleFor(x => x.DriverPhone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits (e.g. 9876543210).");

            // Vehicle: required
            RuleFor(x => x.DriverVehicle)
                .NotEmpty().WithMessage("Vehicle information is required.")
                .MaximumLength(100).WithMessage("Vehicle info cannot exceed 100 characters.");

            // Email: required and valid format
            RuleFor(x => x.DriverEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be in a valid format (e.g. driver@example.com).")
                .MaximumLength(255);
        }
    }

    public class UpdateDriverDtoValidator : AbstractValidator<UpdateDriverDto>
    {
        public UpdateDriverDtoValidator()
        {
            When(x => !string.IsNullOrEmpty(x.DriverName), () =>
            {
                RuleFor(x => x.DriverName)
                    .Matches(@"^[a-zA-Z\s]+$").WithMessage("Driver name must contain alphabets only.")
                    .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.DriverPhone), () =>
            {
                RuleFor(x => x.DriverPhone)
                    .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits.");
            });

            When(x => !string.IsNullOrEmpty(x.DriverVehicle), () =>
            {
                RuleFor(x => x.DriverVehicle)
                    .MaximumLength(100).WithMessage("Vehicle info cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.DriverEmail), () =>
            {
                RuleFor(x => x.DriverEmail)
                    .EmailAddress().WithMessage("Email must be in a valid format.")
                    .MaximumLength(255);
            });
        }
    }
}