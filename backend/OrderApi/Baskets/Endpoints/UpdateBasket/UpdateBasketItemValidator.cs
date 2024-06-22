namespace OrderApi.Baskets.Endpoints.UpdateBasket;

public class UpdateBasketItemValidator : AbstractValidator<UpdateBasketBodyItem>
{
    public UpdateBasketItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
