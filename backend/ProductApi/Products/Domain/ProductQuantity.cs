namespace ProductApi.Products.Domain;

public sealed class ProductQuantity(int value) : NewType<ProductQuantity, int>(value);
