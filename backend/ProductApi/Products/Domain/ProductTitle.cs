namespace ProductApi.Products.Domain;

public record ProductTitle(string Value) : NewValueType<ProductTitle, string>(Value);
