using FluentValidation;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Validators.MenuItem;

public class CreateMenuItemDtoValidator
    : AbstractValidator<CreateMenuItemDto>
{
    public CreateMenuItemDtoValidator()
    {
        RuleFor(x => x.ItemName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ItemPrice)
            .GreaterThan(0);

        RuleFor(x => x.RestaurantId)
            .GreaterThan(0);
    }
}