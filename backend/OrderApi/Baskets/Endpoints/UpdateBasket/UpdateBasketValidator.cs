namespace OrderApi.Baskets.Endpoints.UpdateBasket;

public class UpdateBasketValidator : AbstractValidator<UpdateBasketBody>
{
    public UpdateBasketValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .Must(NotTooManyItems).WithMessage("Too many items are allowed");
    }

    public static bool NotTooManyItems(UpdateBasketItemBody[] items) =>
        items.Length <= 100;
}
