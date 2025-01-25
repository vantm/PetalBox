using Common.Domain.ValueObjects;

namespace ProductApi.Products.Domain;

public record ProductPrice(decimal Value) : NewValueType<ProductPrice, decimal>(Value)
{
    public static readonly ProductPrice Zero = new(0);
}
