using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints.CreateProduct;

public readonly struct CreateProductEndpoint
{
    public static Eff<RT, IResult> New<RT>(CreateProductRequest body) where RT : IAppRuntime =>
        from rt in runtime<RT>()
        from _0 in guard(notnull(body), () => AppErrors.ValidationError("Body", "The request body must not be null"))
        from val in rt.RequiredService<IValidator<CreateProductRequest>>()
        from res in liftEff(() => val.Validate(body))
        from _1 in guard(res.IsValid, AppErrors.ValidationError(res.ToDictionary()))
        from time in rt.RequiredService<TimeProvider>()
        from prod in liftEff(() => Product.New(new(body.Title), new(body.Price), time))
        from repo in rt.RequiredService<IProductRepo<RT>>()
        from _2 in repo.insert(prod)
        from map in rt.RequiredService<ProductMapper>()
        from dto in liftEff(() => map.MapToProductDto(prod))
        select Results.CreatedAtRoute("GetProduct", new { dto.Id }, dto);
}

