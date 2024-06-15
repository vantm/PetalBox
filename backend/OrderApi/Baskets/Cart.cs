namespace OrderApi.Baskets;

public record Cart
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Set<CartItem> Items { get; init; }

    public Cart AddItem(CartItem item)
    {
        return this with
        {
            Items = Items.Add(item)
        };
    }
}
