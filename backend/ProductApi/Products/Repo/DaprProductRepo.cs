using Common.Dapr;
using Common.Domain;
using Common.Domain.ValueObjects;
using Dapr.Client;
using Microsoft.Extensions.Options;
using ProductApi.Products.Domain;

namespace ProductApi.Products.Repo;

public readonly struct DaprProductRepo<RT> : IProductRepo<RT> where RT : IAppRuntime
{
    public Eff<RT, Product> of(ProductId id) =>
        from rt in runtime<RT>()
        from opts in rt.RequiredService<IOptions<CommonOptions>>()
        from req in liftEff(() => PrepareFindStatement(id, opts.Value))
        from dapr in rt.RequiredService<DaprClient>()
        from cancelToken in rt.CancellationToken
        from res in liftEff(() => dapr.InvokeBindingAsync(req, cancelToken))
        from pro in liftEff(() => res.Match(rows => rows.Length != 0 ? Some(Product.Restore(rows[0])) : None))
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

    public Eff<RT, Unit> insert(Product product) =>
        from rt in runtime<RT>()
        from _0 in guard(notnull(product), Error.New(new ArgumentNullException(nameof(product))))
        from opts in rt.RequiredService<IOptions<CommonOptions>>()
        from req in liftEff(() => PrepareInsertStatement(product, opts.Value))
        from dapr in rt.RequiredService<DaprClient>()
        from cancelToken in rt.CancellationToken
        from res in liftEff(() => dapr.InvokeBindingAsync(req, cancelToken))
        from _1 in guardnot(res.GetAffectedRows() == 0, AppErrors.DbError("Insert.Failed"))
        from bus in rt.RequiredService<IServiceBus>()
        from _2 in retry(Schedule.TimeSeries(Constants.Retries),
            from __0 in bus.PublishEventsAsync(product)
            select unit
        )
        select unit;

    private static BindingRequest PrepareInsertStatement(Product product, CommonOptions options)
    {
        const string SqlText = """
            INSERT INTO product (id, title, price, is_active, quantity)
            VALUES ($1, $2, $3, $4, $5)
            """;

        return new BindingRequest(options.BindingName, "exec")
            .AddSqlText(SqlText)
            .AddSqlParams(
                product.Id.Value,
                product.Title.Value,
                product.Price.Value,
                product.IsActive,
                product.Quantity.Value);
    }


    public Eff<RT, Unit> update(Product product) =>
        from rt in runtime<RT>()
        from cancelToken in rt.CancellationToken
        from _0 in guard(notnull(product), Error.New(new ArgumentNullException(nameof(product))))
        from opts in rt.RequiredService<IOptions<CommonOptions>>()
        from req in liftEff(() => PrepareUpdateStatement(product, opts.Value))
        from dapr in rt.RequiredService<DaprClient>()
        from res in liftEff(() => dapr.InvokeBindingAsync(req, cancelToken))
        from _1 in guardnot(res.GetAffectedRows() == 0, AppErrors.DbError("Update.Failed"))
        from bus in rt.RequiredService<IServiceBus>()
        from pubRes in retry(Schedule.TimeSeries(Constants.Retries),
            from __0 in bus.PublishEventsAsync(product)
            select unit
        )
        select unit;

    private static BindingRequest PrepareUpdateStatement(Product product, CommonOptions options)
    {
        const string SqlText = """
            UPDATE product
            SET title = $2, price = $3, is_active= $4, quantity = $5
            WHERE id = $1
            """;

        return new BindingRequest(options.BindingName, "exec")
            .AddSqlText(SqlText)
            .AddSqlParams(
                product.Id.Value,
                product.Title.Value,
                product.Price.Value,
                product.IsActive,
                product.Quantity.Value);
    }
}
