namespace ProductApi.Products.Domain;

public record ProductPrice(decimal Value) : NewValueType<ProductPrice, decimal>(Value);
