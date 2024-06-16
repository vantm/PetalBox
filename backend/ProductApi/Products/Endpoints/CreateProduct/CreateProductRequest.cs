namespace ProductApi.Products.Endpoints.CreateProduct;

public class CreateProductRequest
{
    public string Title { get; init; } = string.Empty;
    public decimal Price { get; init; }
}

