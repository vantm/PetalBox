namespace CustomerApi.Addresses;

public record AddressCreated(Guid Id, Guid UserId) : DomainEvent;
