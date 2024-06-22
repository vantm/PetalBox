namespace OrderApi.Baskets.Domain;

public record Basket(BasketId Id, UserId UserId, IEnumerable<BasketItem> Items)
{
    public static Basket New(UserId userId)
        => New(userId, []);

    public static Basket New(UserId userId, IEnumerable<BasketItem> items)
        => new(BasketId.NewId(), userId, items);
}

