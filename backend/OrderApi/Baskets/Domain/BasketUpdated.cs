namespace OrderApi.Baskets.Domain;

public record BasketUpdated(Guid Id, Guid UserId) : DomainEvent;
