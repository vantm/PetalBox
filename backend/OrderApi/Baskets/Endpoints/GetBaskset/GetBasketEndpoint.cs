using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Baskets.Domain;
using OrderApi.Baskets.Services;

namespace OrderApi.Baskets.Endpoints.GetBaskset;

public record GetBasketEndpoint(
    [FromQuery] Guid UserId,
    IBasketService Baskets,
    CancellationToken RequestAborted)
{
    public async ValueTask<Results<Ok<Basket>, NotFound>> Handle()
    {
        if (UserId == Guid.Empty)
        {
            return TypedResults.NotFound();
        }

        var basket = await Baskets.GetAsync(UserId, RequestAborted);
        if (basket is null)
        {
            basket = new Basket(Guid.NewGuid(), UserId, []);

            await Baskets.SaveAsync(basket, RequestAborted);
        }

        return TypedResults.Ok(basket);
    }
}
