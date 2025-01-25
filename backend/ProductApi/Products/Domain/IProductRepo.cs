using Common.Domain;
using Common.Domain.ValueObjects;

namespace ProductApi.Products.Domain;

public interface IProductRepo<RT> : IRepo<Product, ProductId, RT> where RT : IAppRuntime
{
}
