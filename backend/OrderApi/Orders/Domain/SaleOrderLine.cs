namespace OrderApi.Orders.Domain;

public record SaleOrderLine(
    Guid Id,
    ProductId ProductId,
    string Name,
    int Quantity,
    decimal Price);
