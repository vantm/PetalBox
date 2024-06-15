namespace ProductApi.Products.Domain;

public sealed class ProductId(Guid value) : NewType<ProductId, Guid>(value);
