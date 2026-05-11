using FluentValidation;
using FoodDelivery.API.DTOs.Tushar;

namespace FoodDelivery.API.Validators.Tushar
{
    public class CreateDriverDtoValidator : AbstractValidator<CreateDriverDto>
    {
        public CreateDriverDtoValidator()
        {
            RuleFor(x => x.DriverName)
                .NotEmpty()
                .WithMessage("Driver name is required")
                .MaximumLength(255);

            RuleFor(x => x.DriverPhone)
                .NotEmpty()
                .WithMessage("Driver phone is required")
                .MaximumLength(20);

            RuleFor(x => x.DriverVehicle)
                .NotEmpty()
                .WithMessage("Driver vehicle is required")
                .MaximumLength(255);

            RuleFor(x => x.DriverEmail)
                .NotEmpty()
                .WithMessage("Driver email is required")
                .MaximumLength(255)
                .EmailAddress()
                .WithMessage("Invalid driver email");
        }
    }

    public class UpdateDriverDtoValidator : AbstractValidator<UpdateDriverDto>
    {
        public UpdateDriverDtoValidator()
        {
            RuleFor(x => x.DriverName)
                .NotEmpty()
                .WithMessage("Driver name is required")
                .MaximumLength(255);

            RuleFor(x => x.DriverPhone)
                .NotEmpty()
                .WithMessage("Driver phone is required")
                .MaximumLength(20);

            RuleFor(x => x.DriverVehicle)
                .NotEmpty()
                .WithMessage("Driver vehicle is required")
                .MaximumLength(255);

            RuleFor(x => x.DriverEmail)
                .NotEmpty()
                .WithMessage("Driver email is required")
                .MaximumLength(255)
                .EmailAddress()
                .WithMessage("Invalid driver email");
        }
    }
}