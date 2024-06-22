namespace OrderApi.Checkout.Domain;

public record CheckoutId(Guid Value) : NewValueType<CheckoutId, Guid>(Value);

public record CheckoutRequest(
    CheckoutId Id,
    UserId UserId,
    IEnumerable<CheckoutLine> Lines)
{
}
