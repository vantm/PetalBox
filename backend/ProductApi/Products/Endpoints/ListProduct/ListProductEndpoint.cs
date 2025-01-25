using Common.Models;
using ProductApi.Products.Services;
using System.Text.Json;

namespace ProductApi.Products.Endpoints.ListProduct;

public readonly struct ListProductEndpoint
{
    public static Eff<RT, IResult> New<RT>(PageParams pageParams) where RT : IAppRuntime =>
        from rt in runtime<RT>()
        from log in rt.RequiredService<ILogger<ListProductEndpoint>>()
        from _log1 in liftEff(() =>
        {
            if (log.IsEnabled(LogLevel.Debug))
            {
                log.LogDebug(
                    "Query Params: {Params}",
                    JsonSerializer.Serialize(pageParams));
            }
        })
        from validator in rt.RequiredService<IValidator<PageParams>>()
        from validationResult in SuccessEff(validator.Validate(pageParams))
        from __0 in guard(validationResult.IsValid, () => AppErrors.ValidationError(validationResult.ToDictionary()))
        from repo in rt.RequiredService<IProductQueryService<RT>>()
        from dtos in repo.all(SelectParams.FromPaging(pageParams))
        select Results.Ok(dtos);
}