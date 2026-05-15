using FluentValidation;

namespace FoodDelivery.API.Validators
{
    public class RegisterCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public RegisterCustomerDtoValidator()
        {
            // Name: required, alphabets and spaces only
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Customer name must contain alphabets only (no numbers or special characters).");

            // Email: required and valid format
            RuleFor(x => x.CustomerEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be in a valid format (e.g. user@example.com).");

            // Phone: required, exactly 10 digits
            RuleFor(x => x.CustomerPhone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits (e.g. 9876543210).");

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

    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            When(x => !string.IsNullOrEmpty(x.CustomerName), () =>
            {
                RuleFor(x => x.CustomerName)
                    .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name must contain alphabets only.")
                    .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.CustomerEmail), () =>
            {
                RuleFor(x => x.CustomerEmail)
                    .EmailAddress().WithMessage("Email must be in a valid format (e.g. user@example.com).")
                    .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.CustomerPhone), () =>
            {
                RuleFor(x => x.CustomerPhone)
                    .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits.");
            });

            When(x => !string.IsNullOrEmpty(x.Password), () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                    .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                    .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            });
        }
    }
}
