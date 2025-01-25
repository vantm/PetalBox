using Common.Domain.ValueObjects;
using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints.AdjustProduct;

public readonly struct AdjustProductEndpoint
{
    public static Eff<RT, IResult> New<RT>(Guid id, AdjustProductBody body) where RT : IAppRuntime =>
        from rt in runtime<RT>()
        from _0 in guard(notnull(body), AppErrors.ValidationError("Body", "The request body must be not null."))
        from _1 in guard(notDefault(id), AppErrors.NotFoundError)
        from val in rt.RequiredService<IValidator<AdjustProductBody>>()
        from valRes in liftEff(() => val.Validate(body))
        from _2 in guard(valRes.IsValid, AppErrors.ValidationError(valRes.ToDictionary()))
        from repo in rt.RequiredService<IProductRepo<RT>>()
        from prod in repo.of(ProductId.FromValue(id))
        from time in rt.RequiredService<TimeProvider>()
        from _3 in liftEff(() => prod.Adjust(new(body.Delta), time))
        from _4 in repo.update(prod)
        select Results.NoContent();
}
