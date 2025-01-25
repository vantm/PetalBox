using Common.Domain.ValueObjects;
using ProductApi.Products.Domain;
using ProductApi.Products.Services;
using System.Diagnostics;

namespace ProductApi.Products.Endpoints.GetProduct;

public readonly struct GetProductEndpoint
{
    public static Eff<RT, IResult> New<RT>(Guid id) where RT : IAppRuntime =>
        from rt in runtime<RT>()
        from repo in rt.RequiredService<IProductQueryService<RT>>()
        from logger in rt.RequiredService<ILogger<GetProductEndpoint>>()
        from time in rt.RequiredService<TimeProvider>()
        from __0 in liftEff(() => logger.LogInformation("Finding product {id} has started at {time}", id, time.GetLocalNow()))
        from sw in SuccessEff(Stopwatch.StartNew())
        from dto in repo.of(ProductId.FromValue(id))
        from __1 in liftEff(sw.Stop)
        from __2 in liftEff(() => logger.LogInformation("Finding product {id} has completed in {time} ms", id, sw.ElapsedMilliseconds))
        select Results.Ok(dto);
}

