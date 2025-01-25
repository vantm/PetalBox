using ProductApi.Products.Domain;
using ProductApi.Products.Models;
using Riok.Mapperly.Abstractions;

namespace ProductApi.Products;

[Mapper]
public partial class ProductMapper
{
    public virtual partial ProductDto MapToProductDto(Product product);
    public virtual partial IEnumerable<ProductDto> MapToProductDto(IEnumerable<Product> products);
}
