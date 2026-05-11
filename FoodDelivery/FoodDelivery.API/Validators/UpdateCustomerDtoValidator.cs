using FluentValidation;
using FoodDelivery.API.DTOs.Sanjana;
namespace FoodDelivery.API.Validators
{
    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            When(x => !string.IsNullOrEmpty(x.CustomerName), () =>
            {
                RuleFor(x => x.CustomerName)
                    .MaximumLength(255)
                    .WithMessage("Name cannot exceed 255 characters");
            });

            When(x => !string.IsNullOrEmpty(x.CustomerEmail), () =>
            {
                RuleFor(x => x.CustomerEmail)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");
            });

            When(x => !string.IsNullOrEmpty(x.CustomerPhone), () =>
            {
                RuleFor(x => x.CustomerPhone)
                .Matches(@"^\d{10}$")
                .MaximumLength(10).WithMessage("Phone number cannot exceed more than 10 digits");
            });
        }
    }
}
