using Carter;
using ProductApi.Products.Endpoints;

namespace ProductApi.Products;

public class ProductModule : CarterModule
{
    public ProductModule() : base("/products")
    {
        WithTags("Product");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", ([AsParameters] ListProductEndpoint ep) => ep.Handle())
           .WithName(nameof(ListProductEndpoint))
           .WithOpenApi();

        app.MapGet("{id:guid}", ([AsParameters] GetProductEndpoint ep) => ep.Handle())
           .WithName(nameof(GetProductEndpoint))
           .WithOpenApi();

        app.MapPost("", ([AsParameters] CreateProductEndpoint ep) => ep.Handle())
           .WithName(nameof(CreateProductEndpoint))
           .WithOpenApi();

        app.MapPost("{id:guid}/quantity", ([AsParameters] AdjustProductEndpoint ep) => ep.Handle())
           .WithName(nameof(AdjustProductEndpoint))
           .WithOpenApi();
    }
}
