﻿using LanguageExt.Effects.Traits;
using ProductApi.Products.Domain;
using System.Text.Json;

namespace ProductApi.Products.Endpoints;

public readonly struct ListProductEndpoint<RT>
    where RT : struct, HasCancel<RT>, HasServiceProvider
{
    public const string Name = "ListProduct";

    public static Aff<RT, IResult> New(PageParams pageParams) =>
        from rt in runtime<RT>()
        from log in rt.RequiredService<ILogger<ListProductEndpoint<RT>>>()
        from _log1 in tap(() =>
        {
            if (log.IsEnabled(LogLevel.Debug))
            {
                log.LogDebug("List product query params: {params}", JsonSerializer.Serialize(pageParams));
            }
        })
        from repo in rt.RequiredService<IProductRepo<RT>>()
        from validator in rt.RequiredService<IValidator<PageParams>>()
        from validationResult in SuccessEff(validator.Validate(pageParams))
        from __0 in guard(validationResult.IsValid, () => AppErrors.ValidationError(validationResult.ToDictionary()))
        from res in repo.all(SelectParams.FromPaging(pageParams))
        select Results.Ok(res);
}