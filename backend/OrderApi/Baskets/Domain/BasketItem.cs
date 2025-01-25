using Common.Domain.ValueObjects;

namespace OrderApi.Baskets.Domain;

public record BasketItem(ProductId ProductId, int Quantity);

