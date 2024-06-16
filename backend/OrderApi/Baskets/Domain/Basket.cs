namespace OrderApi.Baskets.Domain;

public record Basket(Guid Id, Guid UserId, IEnumerable<BasketItem> Items);

