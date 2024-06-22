namespace OrderApi.Orders.Domain;

public record SaleOrderCreated(Guid Id, Guid UserId) : DomainEvent;
