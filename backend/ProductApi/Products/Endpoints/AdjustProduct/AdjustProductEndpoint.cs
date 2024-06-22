using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints.AdjustProduct;

public readonly struct AdjustProductEndpoint<RT>
    where RT : struct, HasCancel<RT>, HasServiceProvider
{
    public static Aff<RT, IResult> New(Guid id, AdjustProductBody body) =>
        from rt in runtime<RT>()
        from __0 in guard(notnull(body), AppErrors.ValidationError("Body", "The request body must be not null."))
        from __1 in guard(notDefault(id), AppErrors.NotFoundError)
        from validator in rt.RequiredService<IValidator<AdjustProductBody>>()
        from validationResult in SuccessEff(validator.Validate(body))
        from __2 in guard(validationResult.IsValid, AppErrors.ValidationError(validationResult.ToDictionary()))
        from repo in rt.RequiredService<IProductRepo<RT>>()
        from product in repo.of(ProductId.FromValue(id))
        from time in rt.RequiredService<TimeProvider>()
        from adjustedProduct in SuccessEff(product.Adjust(new(body.Delta), time))
        from __4 in repo.update(adjustedProduct)
        select Results.NoContent();
}
