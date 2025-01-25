using Common.Dapr;
using Common.Domain.ValueObjects;
using Common.Models;
using Dapr.Client;
using Microsoft.Extensions.Options;
using ProductApi.Products.Models;
using System.Text.Json;

namespace ProductApi.Products.Services.Internals;

public readonly struct DaprProductQueryService<RT> : IProductQueryService<RT> where RT : IAppRuntime
{
    public Eff<RT, IEnumerable<ProductDto>> all(SelectParams @params) =>
        from rt in runtime<RT>()
        from opts in rt.RequiredService<IOptions<CommonOptions>>()
        from log in rt.RequiredService<ILogger<DaprProductQueryService<RT>>>()
        from _log1 in liftEff(() => log.LogDebug("Preparing for querying products"))
        from req in liftEff(() => PrepareSelectStatement(@params, opts.Value))
        from _log2 in liftEff(() =>
        {
            if (log.IsEnabled(LogLevel.Debug))
            {
                log.LogDebug("Prepared stmt: {SQL}", req.Metadata["sql"]);
            }
        })
        from dapr in rt.RequiredService<DaprClient>()
        from cancelToken in rt.CancellationToken
        from res in liftEff(() => dapr.InvokeBindingAsync(req, cancelToken))
        from _log3 in liftEff(() =>
        {
            if (log.IsEnabled(LogLevel.Debug))
            {
                log.LogDebug("Binding response: {Response}", JsonSerializer.Serialize(res.Metadata));
            }
        })
        from ret in liftEff(() => res.Match(rows => rows.Select(ReadResponse)))
        select ret;


    private static BindingRequest PrepareSelectStatement(SelectParams @params, CommonOptions options)
    {
        const string SqlText = """
            SELECT id, title, price, is_active, quantity
            FROM product
            OFFSET $1
            LIMIT $2
            """;

        return new BindingRequest(options.BindingName, "query")
            .AddSqlText(SqlText)
            .AddSqlParams(@params.Offset, @params.Limit);
    }

    public Eff<RT, ProductDto> of(ProductId id) =>
        from rt in runtime<RT>()
        from opts in rt.RequiredService<IOptions<CommonOptions>>()
        from req in liftEff(() => PrepareFindStatement(id, opts.Value))
        from dapr in rt.RequiredService<DaprClient>()
        from cancelToken in rt.CancellationToken
        from res in liftEff(() => dapr.InvokeBindingAsync(req, cancelToken))
        from pro in liftEff(() => res.Match(rows => rows.Length != 0 ? Some(ReadResponse(rows[0])) : None))
        from ret in pro.ToEff(AppErrors.NotFoundError)
        select ret;

    private static BindingRequest PrepareFindStatement(ProductId id, CommonOptions options)
    {
        const string SqlText = """
            SELECT id, title, price, is_active, quantity
            FROM product
            WHERE id = $1
            """;

        return new BindingRequest(options.BindingName, "query")
            .AddSqlText(SqlText)
            .AddSqlParams(id.Value);
    }

    private static ProductDto ReadResponse(JsonDocument[] row)
    {
        var productId = Guid.Parse(row[0].Deserialize<string>()!);
        return new(
            productId,
            row[1].Deserialize<string>()!,
            row[2].Deserialize<decimal>()!,
            row[4].Deserialize<int>()!,
            row[3].Deserialize<bool>()!);
    }
}
