using Common.Domain;
using Common.Domain.ValueObjects;

namespace OrderApi.Orders.Domain;

public record SaleOrderCreated(SaleOrderId Id, UserId UserId) : DomainEvent;
