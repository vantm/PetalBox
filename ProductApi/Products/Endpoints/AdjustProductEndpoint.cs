using LanguageExt.Effects.Traits;
using ProductApi.Products.Domain;

namespace ProductApi.Products.Endpoints;

public class AdjustProductRequest
{
    public int Delta { get; init; }
}

public class AdjustProductValidator : AbstractValidator<AdjustProductRequest>
{
    public AdjustProductValidator()
    {
        RuleFor(x => x.Delta).NotEmpty();
    }
}

public readonly struct AdjustProductEndpoint<RT>
    where RT : struct, HasCancel<RT>, HasServiceProvider
{
    public static Aff<RT, IResult> New(Guid id, AdjustProductRequest body) =>
        from rt in runtime<RT>()
        from __0 in guard(notnull(body), AppErrors.ValidationError("Body", "The request body must be not null."))
        from __1 in guard(notDefault(id), AppErrors.NotFoundError)
        from validator in rt.RequiredService<IValidator<AdjustProductRequest>>()
        from validationResult in SuccessEff(validator.Validate(body))
        from __2 in guard(validationResult.IsValid, AppErrors.ValidationError(validationResult.ToDictionary()))
        from repo in rt.RequiredService<IProductRepo<RT>>()
        from product in repo.of(id)
        from time in rt.RequiredService<TimeProvider>()
        from __3 in tap(() => { product.Adjust(body.Delta, time); })
        from __4 in repo.update(product)
        select Results.NoContent();
}
