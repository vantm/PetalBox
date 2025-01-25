using Common.Domain.ValueObjects;

namespace OrderApi.Checkout.Domain;

public record CheckoutRequest(
    CheckoutId Id,
    UserId UserId,
    IEnumerable<CheckoutLine> Lines)
{
}
