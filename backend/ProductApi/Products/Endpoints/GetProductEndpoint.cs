using ProductApi.Products.Domain;
using System.Diagnostics;

namespace ProductApi.Products.Endpoints;

public readonly struct GetProductEndpoint<RT>
    where RT : struct, HasCancel<RT>, HasServiceProvider
{
    public static Aff<RT, IResult> New(Guid id) =>
        from rt in runtime<RT>()
        from repo in rt.RequiredService<IProductRepo<RT>>()
        from logger in rt.RequiredService<ILogger<GetProductEndpoint<RT>>>()
        from time in rt.RequiredService<TimeProvider>()
        from __0 in tap(() => logger.LogInformation("Finding product {id} has started at {time}", id, time.GetLocalNow()))
        from sw in SuccessEff(Stopwatch.StartNew())
        from product in repo.of(new(id))
        from __1 in tap(() => sw.Stop())
        from __2 in tap(() => logger.LogInformation("Finding product {id} has completed in {time} ms", id, sw.ElapsedMilliseconds))
        from mapper in rt.RequiredService<Mapper>()
        from dto in mapper.MapToProductDtoEff(product)
        select Results.Ok(dto);
}

