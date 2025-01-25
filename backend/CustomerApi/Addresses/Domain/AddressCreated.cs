namespace CustomerApi.Addresses.Domain;

public record AddressCreated(Guid Id, Guid CustomerId, Guid UserId) : DomainEvent;

