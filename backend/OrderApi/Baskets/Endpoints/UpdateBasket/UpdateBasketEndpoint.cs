using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Baskets.Domain;
using OrderApi.Baskets.Services;

namespace OrderApi.Baskets.Endpoints.UpdateBasket;

public record UpdateBasketEndpoint(
    [FromQuery(Name = "userId")] Guid Uid,
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

        var userId = UserId.FromValue(Uid);

        var basket = await Baskets.GetAsync(userId, RequestAborted) ?? Basket.New(userId);

        var nextCartState = basket with
        {
            Items = Body.Items.Select(CreateBasketItemFromBody)
        };

        await Baskets.SaveAsync(nextCartState, RequestAborted);

        return TypedResults.NoContent();
    }

    private static BasketItem CreateBasketItemFromBody(UpdateBasketBodyItem item)
        => new(ProductId.FromValue(item.ProductId), item.Quantity);
}

