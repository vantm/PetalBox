using Dapr.Client;
using ProductApi.Products.Domain;

namespace ProductApi.Products.Repo;

public class DaprProductRepo(
    DaprClient dapr,
    DaprProductRepoHelper helper,
    IServiceBus serviceBus) : IProductRepo
{
    public Task<Either<Error, IEnumerable<Product>>> SelectAsync(
        SelectParams @params, CancellationToken cancellationToken = default)
        => EitherWithError(@params)
        .Map(helper.PrepareSelectStatement)
        .MapAsync(req => dapr.InvokeBindingAsync(req, cancellationToken))
        .BindAsync(res => res.Match(rows => rows.Select(Product.FromJsonArray)));

    public Task<Either<Error, Product>> FindAsync(
        Guid id, CancellationToken cancellationToken = default)
        => EitherWithError(id)
        .Map(helper.PrepareFindStatement)
        .MapAsync(req => dapr.InvokeBindingAsync(req, cancellationToken))
        .BindAsync(res => res.Match((rows) => rows.Length == 0 ? null : Product.FromJsonArray(rows[0])))
        .LeftWhenAsync(isDefault, () => Error.New("Product.NotFound"))!;

    public Task<Either<Error, Unit>> InsertAsync(
        Product product, CancellationToken cancellationToken = default)
        => EitherWithError(product)
        .LeftWhen(isDefault, () => Error.New("Product.Null"))
        .Map(helper.PrepareInsertStatement)
        .MapAsync(req => dapr.InvokeBindingAsync(req, cancellationToken))
        .LeftWhenAsync(res => res.GetAffectedRows() == 0, () => Error.New("Insert.Failed"))
        .BindAsync(_ => serviceBus.PublishEventsAsync(product))
        .ToUnitAsync();

    public Task<Either<Error, Unit>> UpdateAsync(
        Product product, CancellationToken cancellationToken = default)
        => EitherWithError(product)
        .LeftWhen(isDefault, () => Error.New("Product.Null"))
        .Map(helper.PrepareUpdateStatement)
        .MapAsync(req => dapr.InvokeBindingAsync(req, cancellationToken))
        .LeftWhenAsync(res => res.GetAffectedRows() == 0, () => Error.New("Update.Failed"))
        .BindAsync(_ => serviceBus.PublishEventsAsync(product))
        .ToUnitAsync();
}
