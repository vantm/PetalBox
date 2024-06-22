namespace ProductApi.Products.Domain;

public record ProductQuantity(int Value) : NewValueType<ProductQuantity, int>(Value);
