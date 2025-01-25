namespace CustomerApi.Addresses.Domain;

public record AddressUpdated(Guid Id, Guid CustomerId, Guid UserId) : DomainEvent;
