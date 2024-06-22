namespace OrderApi.Baskets.Domain;

public record BasketUpdated(BasketId Id, UserId UserId) : DomainEvent;
