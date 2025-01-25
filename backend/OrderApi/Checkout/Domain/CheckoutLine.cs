using Common.Domain.ValueObjects;

namespace OrderApi.Checkout.Domain;

public record CheckoutLine(ProductId ProductId, int Quantity);
