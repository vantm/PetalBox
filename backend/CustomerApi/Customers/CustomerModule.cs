using Carter;

namespace CustomerApi.Customers;

public class CustomerModule : CarterModule
{
    public CustomerModule() : base("/customers")
    {
        WithTags("Customer");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
    }
}
