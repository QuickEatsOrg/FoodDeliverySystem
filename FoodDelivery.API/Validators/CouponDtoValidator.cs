using FluentValidation;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Validators
{
    public class CreateCouponDtoValidator : AbstractValidator<CreateCouponDto>
    {
        public CreateCouponDtoValidator()
        {
            RuleFor(x => x.CouponCode)
                .NotEmpty()
                .WithMessage("Coupon code is required")
                .MaximumLength(20)
                .WithMessage("Coupon code cannot exceed 20 characters");

            RuleFor(x => x.DiscountAmount)
                .GreaterThan(0)
                .WithMessage("Discount amount must be greater than 0");

            RuleFor(x => x.ExpiryDate)
                .NotEmpty()
                .WithMessage("Expiry date is required")
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Expiry date must be a future date");
        }
    }
    public class UpdateCouponDtoValidator : AbstractValidator<UpdateCouponDto>
    {
        public UpdateCouponDtoValidator()
        {
            RuleFor(x => x.CouponCode)
                .NotEmpty()
                .WithMessage("Coupon code is required")
                .MaximumLength(20)
                .WithMessage("Coupon code cannot exceed 20 characters");

            RuleFor(x => x.DiscountAmount)
                .GreaterThan(0)
                .WithMessage("Discount amount must be greater than 0");

            RuleFor(x => x.ExpiryDate)
                .NotEmpty()
                .WithMessage("Expiry date is required")
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Expiry date must be a future date");
        }
    }
}
