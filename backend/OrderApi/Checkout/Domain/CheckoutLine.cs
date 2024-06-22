namespace OrderApi.Checkout.Domain;

public record CheckoutLine(ProductId ProductId, int Quantity);
