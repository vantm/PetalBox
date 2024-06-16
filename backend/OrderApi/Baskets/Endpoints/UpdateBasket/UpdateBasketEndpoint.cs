using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Baskets.Domain;
using OrderApi.Baskets.Services;

namespace OrderApi.Baskets.Endpoints.UpdateBasket;

public record UpdateBasketEndpoint(
    [FromQuery] Guid UserId,
    UpdateBasketBody Body,
    IValidator<UpdateBasketBody> Validator,
    IBasketService Baskets,
    CancellationToken RequestAborted)
{
    public async ValueTask<Results<NoContent, ValidationProblem>> Handle()
    {
        var validationResult = Validator.Validate(Body);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var basket = await Baskets.GetAsync(UserId, RequestAborted)
            ?? new Basket(Guid.NewGuid(), UserId, []);

        var nextCartState = basket with
        {
            Items = Body.Items.Select(x => new BasketItem(x.ProductId, x.Quantity))
        };

        await Baskets.SaveAsync(nextCartState, RequestAborted);

        return TypedResults.NoContent();
    }
}

