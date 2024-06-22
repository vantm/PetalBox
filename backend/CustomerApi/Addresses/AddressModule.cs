using Carter;

namespace CustomerApi.Addresses;

public class AddressModule : CarterModule
{
    public AddressModule() : base("/addresses")
    {
        WithTags("Address");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", () =>
        {

        });
    }
}
