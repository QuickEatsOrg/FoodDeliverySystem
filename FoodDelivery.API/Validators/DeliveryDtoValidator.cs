using FluentValidation;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Validators.Tushar
{
    public class AssignDriverDtoValidator : AbstractValidator<AssignDriverDto>
    {
        public AssignDriverDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("Order id must be greater than 0");

            RuleFor(x => x.DriverId)
                .GreaterThan(0)
                .WithMessage("Driver id must be greater than 0");
        }
    }

    public class UpdateDeliveryStatusDtoValidator : AbstractValidator<UpdateDeliveryStatusDto>
    {
        public UpdateDeliveryStatusDtoValidator()
        {
            RuleFor(x => x.OrderStatus)
                .NotEmpty()
                .WithMessage("Order status is required")
                .MaximumLength(50)
                .WithMessage("Order status cannot exceed 50 characters");
        }
    }

    public class DriverLoginDtoValidator : AbstractValidator<DriverLoginDto>
    {
        public DriverLoginDtoValidator()
        {
            RuleFor(x => x.DriverEmail)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}