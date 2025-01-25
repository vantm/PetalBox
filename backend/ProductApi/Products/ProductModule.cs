using Carter;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Products.Endpoints.AdjustProduct;
using ProductApi.Products.Endpoints.CreateProduct;
using ProductApi.Products.Endpoints.GetProduct;
using ProductApi.Products.Endpoints.ListProduct;

namespace ProductApi.Products;

public class ProductModule : CarterModule
{
    public ProductModule() : base("/products")
    {
        WithTags("Product");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", ([AsParameters] PageParams qs, [FromServices] IAppRuntime rt) =>
            ListProductEndpoint.New<IAppRuntime>(qs).RunAsync(rt));

        app.MapGet("{id:guid}", (Guid id, IAppRuntime rt) =>
            GetProductEndpoint.New<IAppRuntime>(id).RunAsync(rt))
           .WithName("GetProduct");

        app.MapPost("", (CreateProductRequest body, IAppRuntime rt) =>
            CreateProductEndpoint.New<IAppRuntime>(body).RunAsync(rt));

        app.MapPost("{id:guid}/quantity", (Guid id, AdjustProductBody body, AppRuntime rt) =>
            AdjustProductEndpoint.New<IAppRuntime>(id, body).RunAsync(rt));
    }
}
