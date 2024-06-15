namespace OrderApi.Baskets;

public class CartItem : Record<CartItem>
{
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }

    public override int CompareTo(CartItem other)
    {
        return ProductId.CompareTo(other.ProductId);
    }
}
