using Common.Domain.ValueObjects;

namespace OrderApi.Checkout.Domain;

public record CheckoutId(Guid Value) : GuidId<CheckoutId>(Value);
