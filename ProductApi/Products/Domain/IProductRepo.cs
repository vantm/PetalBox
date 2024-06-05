namespace ProductApi.Products.Domain;

public interface IProductRepo
{
    Task<Either<Error, IEnumerable<Product>>> SelectAsync(
        SelectParams @params, CancellationToken cancellationToken = default);

    Task<Either<Error, Product>> FindAsync(
        Guid id, CancellationToken cancellationToken = default);

    Task<Either<Error, Unit>> InsertAsync(
        Product product, CancellationToken cancellationToken = default);

    Task<Either<Error, Unit>> UpdateAsync(
        Product product, CancellationToken cancellationToken = default);
}
