using Common.Domain.ValueObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Baskets.Domain;
using OrderApi.Baskets.Services;

namespace OrderApi.Baskets.Endpoints.GetBaskset;

public record GetBasketEndpoint(
    [FromQuery(Name = "userId")] Guid Uid,
    IBasketService Baskets,
    CancellationToken RequestAborted)
{
    public async ValueTask<Results<Ok<Basket>, NotFound>> Handle()
    {
        if (Uid == Guid.Empty)
        {
            return TypedResults.NotFound();
        }

        var userId = UserId.FromValue(Uid);
        var basket = await Baskets.GetAsync(userId, RequestAborted);
        if (basket is null)
        {
            basket = Basket.New(userId);
            await Baskets.SaveAsync(basket, RequestAborted);
        }

        return TypedResults.Ok(basket);
    }
}

