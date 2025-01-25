using Common.Domain.ValueObjects;

namespace ProductApi.Products.Domain;

public record ProductTitle(string Value) : NewValueType<ProductTitle, string>(Value)
{
    public static readonly ProductTitle InitialValue = new("New product");
}
