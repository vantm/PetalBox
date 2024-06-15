namespace ProductApi.Products.Models;

public record ProductDto(
    Guid Id, string Title, decimal Price, int Quantity, bool IsActive);


