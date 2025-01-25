using Common.Domain.ValueObjects;
using Common.Models;
using ProductApi.Products.Models;

namespace ProductApi.Products.Services;

public interface IProductQueryService<RT> where RT : IAppRuntime
{
    Eff<RT, IEnumerable<ProductDto>> all(SelectParams @params);
    Eff<RT, ProductDto> of(ProductId id);
}
