
using Carter;
using OrderApi.Baskets.Endpoints.GetBaskset;
using OrderApi.Baskets.Endpoints.UpdateBasket;

namespace OrderApi.Baskets;

public class BasketModule : CarterModule
{
    public BasketModule() : base("/basket")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", ([AsParameters] GetBasketEndpoint ep) => ep.Handle());

        app.MapPost("", ([AsParameters] UpdateBasketEndpoint ep) => ep.Handle());
    }
}

