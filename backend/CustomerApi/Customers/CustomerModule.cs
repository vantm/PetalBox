using Carter;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Customers;

public class CustomerModule : CarterModule
{
    public CustomerModule() : base("/customers")
    {
        WithTags("Customer");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", ([FromQuery] Guid userId, DaprClient dapr, CancellationToken aborted) =>
        {

        });

        app.MapGet("{id:guid}", (Guid id, [FromQuery] Guid userId, DaprClient dapr, CancellationToken aborted) =>
        {

        });

        app.MapPost("", ([FromQuery] Guid userId, DaprClient dapr, CancellationToken aborted) =>
        {

        });

    }
}

public record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string Phone);