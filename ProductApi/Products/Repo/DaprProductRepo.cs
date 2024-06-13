using Dapr.Client;
using LanguageExt.Effects.Traits;
using ProductApi.Products.Domain;
using System.Text.Json;

namespace ProductApi.Products.Repo;

public class DaprProductRepo<RT> : IProductRepo<RT>
    where RT : struct, HasCancel<RT>, HasServiceProvider
{
    public Aff<RT, Product> of(Guid id) =>
        from rt in runtime<RT>()
        from helper in rt.RequiredService<DaprProductRepoHelper>()
        from req in SuccessEff(helper.PrepareFindStatement(id))
        from dapr in rt.RequiredService<DaprClient>()
        from res in dapr.InvokeBindingAsync(req, rt.CancellationToken).ToAff()
        from parsed in SuccessEff(res.Match((rows) => rows.Length == 0 ? None : Some(Product.FromJsonArray(rows[0]))))
        from ret in flattenEff(parsed)
        select ret;

    public Aff<RT, Seq<Product>> all(SelectParams @params) =>
        from rt in runtime<RT>()
        from log in rt.RequiredService<ILogger<DaprProductRepo<RT>>>()
        from _log1 in tap(() => log.LogDebug("Preparing for querying products"))
        from helper in rt.RequiredService<DaprProductRepoHelper>()
        from req in SuccessEff(helper.PrepareSelectStatement(@params))
        from _log2 in tap(() =>
        {
            if (log.IsEnabled(LogLevel.Debug))
            {
                log.LogDebug("Prepared stmt: {SQL}", req.Metadata["sql"]);
            }
        })
        from dapr in rt.RequiredService<DaprClient>()
        from res in dapr.InvokeBindingAsync(req, rt.CancellationToken).ToAff()
        from _log3 in tap(() =>
        {
            if (log.IsEnabled(LogLevel.Debug))
            {
                log.LogDebug("Binding response: {Response}", JsonSerializer.Serialize(res.Metadata));
            }
        })
        from parsed in SuccessEff(res.Match(rows => rows.Select(Product.FromJsonArray).ToSeq()))
        from ret in flattenEff(parsed)
        select ret;

    public Aff<RT, Unit> insert(Product product) =>
        from rt in runtime<RT>()
        from _0 in guard(notnull(product), Error.New(new ArgumentNullException(nameof(product))))
        from helper in rt.RequiredService<DaprProductRepoHelper>()
        from req in SuccessEff(helper.PrepareInsertStatement(product))
        from dapr in rt.RequiredService<DaprClient>()
        from res in dapr.InvokeBindingAsync(req, rt.CancellationToken).ToAff()
        from _1 in guardnot(res.GetAffectedRows() == 0, AppErrors.DbError("Insert.Failed"))
        from bus in rt.RequiredService<IServiceBus>()
        from pubRes in retry(Schedule.TimeSeries(Constants.Retries),
            from pubRes in bus.PublishEventsAsync(product.DomainEvents).ToAff()
            from _0 in flattenEff(pubRes)
            select pubRes
        )
        from _2 in flattenEff(pubRes)
        select unit;

    public Aff<RT, Unit> update(Product product) =>
        from rt in runtime<RT>()
        from _0 in guard(notnull(product), Error.New(new ArgumentNullException(nameof(product))))
        from helper in rt.RequiredService<DaprProductRepoHelper>()
        from req in SuccessEff(helper.PrepareUpdateStatement(product))
        from dapr in rt.RequiredService<DaprClient>()
        from res in dapr.InvokeBindingAsync(req, rt.CancellationToken).ToAff()
        from _1 in guardnot(res.GetAffectedRows() == 0, AppErrors.DbError("Update.Failed"))
        from bus in rt.RequiredService<IServiceBus>()
        from pubRes in retry(Schedule.TimeSeries(Constants.Retries),
            from pubRes in bus.PublishEventsAsync(product.DomainEvents).ToAff()
            from _0 in flattenEff(pubRes)
            select pubRes
        )
        from _2 in flattenEff(pubRes)
        select unit;
}
