namespace OrderApi.Orders.Domain;

public interface ISaleOrderRepo
{
    Task<Either<Error, Option<SaleOrder>>> FindAsync(SaleOrderId id);
    Task<Either<Error, Unit>> InsertAsync(SaleOrder order);
    Task<Either<Error, Unit>> UpdateAsync(SaleOrder order);
}
