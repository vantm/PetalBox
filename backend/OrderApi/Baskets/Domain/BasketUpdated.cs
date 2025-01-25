using Common.Domain;
using Common.Domain.ValueObjects;

namespace OrderApi.Baskets.Domain;

public record BasketUpdated(BasketId Id, UserId UserId) : DomainEvent;
