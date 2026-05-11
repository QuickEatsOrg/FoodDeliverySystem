
using FluentValidation;
using FoodDelivery.API.DTOs.Sanjana;

namespace FoodDelivery.API.Validators
{
    public class RegisterCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public RegisterCustomerDtoValidator()
        {

            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer name is required");


            RuleFor(x => x.CustomerEmail).EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.CustomerEmail))
                .WithMessage("Invalid email format");


            RuleFor(x => x.CustomerPhone)
                .Matches(@"^\d{10}$")
                .When(x => !string.IsNullOrEmpty(x.CustomerPhone))
                .WithMessage("Customer phone must be a 10-digit number");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.CustomerEmail) || !string.IsNullOrEmpty(x.CustomerPhone))
                .WithMessage("Either CustomerEmail or CustomerPhone must be provided");


            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain atleast one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain atleast one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain atleast one digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain atleast one special character");
        }
    }
}