namespace ProductApi.Products.Domain;

public sealed class ProductPrice(decimal value) :NewType<ProductPrice, decimal>(value);
