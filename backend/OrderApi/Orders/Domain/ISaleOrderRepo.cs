using Common.Domain;
using Common.Domain.ValueObjects;

namespace OrderApi.Orders.Domain;

public interface ISaleOrderRepo<RT> : IRepo<SaleOrder, SaleOrderId, RT> where RT : IAppRuntime
{
}
