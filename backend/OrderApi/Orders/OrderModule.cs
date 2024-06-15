using Carter;

namespace OrderApi.Orders;

public class OrderModule : CarterModule
{
    public OrderModule() : base("/orders")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
    }
}
