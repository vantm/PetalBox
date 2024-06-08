﻿using Carter;
using Microsoft.AspNetCore.Mvc;
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
        app.MapGet(
                "",
                ([AsParameters] PageParams qs, [FromServices] WebRuntime rt) =>
                    rt.RunAff(ListProductEndpoint<Runtime>.New(qs))
            );

        app.MapGet(
                "{id:guid}",
                (Guid id, WebRuntime rt) =>
                    rt.RunAff(GetProductEndpoint<Runtime>.New(id))
            )
           .WithName("GetProduct");

        app.MapPost(
                "",
                (CreateProductRequest body, WebRuntime rt) =>
                    rt.RunAff(CreateProductEndpoint<Runtime>.New(body))
            );

        app.MapPost(
                "{id:guid}/quantity",
                (Guid id, AdjustProductRequest body, WebRuntime rt) =>
                    rt.RunAff(AdjustProductEndpoint<Runtime>.New(id, body))
            );
    }
}
