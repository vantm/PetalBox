namespace ProductApi.Products.Domain;

public sealed class ProductTitle(string value) : NewType<ProductTitle, string>(value);
