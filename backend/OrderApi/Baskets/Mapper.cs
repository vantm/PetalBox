using OrderApi.Baskets.Models;
using Riok.Mapperly.Abstractions;

namespace OrderApi.Baskets;

[Mapper]
public partial class Mapper
{
    public virtual partial CartDto MapToCartDto(Cart cart);
}
