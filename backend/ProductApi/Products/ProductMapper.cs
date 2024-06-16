using ProductApi.Products.Domain;
using ProductApi.Products.Models;
using Riok.Mapperly.Abstractions;

namespace ProductApi.Products;

[Mapper]
public partial class ProductMapper
{
    public virtual partial ProductDto MapToProductDto(Product product);

    public Eff<ProductDto> MapToProductDtoEff(Product product)
        => SuccessEff(MapToProductDto(product));

    public Eff<Seq<ProductDto>> MapToProductDtoEff(Seq<Product> products)
        => SuccessEff(products.Map(MapToProductDto));
}
