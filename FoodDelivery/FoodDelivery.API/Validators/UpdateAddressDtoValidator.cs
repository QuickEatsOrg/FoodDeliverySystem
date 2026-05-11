using FluentValidation;
using FoodDelivery.API.DTOs.Sanjana;

namespace FoodDelivery.API.Validators
{
    public class UpdateAddressDtoValidator : AbstractValidator<UpdateAddressDto>
    {
        public UpdateAddressDtoValidator()
        {
            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Address Line 1 is required")
                .MaximumLength(200).WithMessage("Address Line 1 cannot exceed 200 characters");
            RuleFor(x => x.AddressLine2)
                .MaximumLength(200).WithMessage("Address Line 2 cannot exceed 200 characters");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City cannot exceed 50 characters");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(100).WithMessage("State cannot exceed 50 characters");
            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal Code is required")
                .Matches(@"^\d{6}$").WithMessage("Postal Code must be a 6-digit number");
        }
    }
}
