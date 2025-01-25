using Common.Domain.ValueObjects;

namespace ProductApi.Products.Domain;

public record ProductQuantity(int Value) : NewValueType<ProductQuantity, int>(Value)
{
    public static readonly ProductQuantity Zero = new(0);
}
