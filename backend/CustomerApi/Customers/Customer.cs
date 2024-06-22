namespace CustomerApi.Customers;

public record CustomerId(Guid Value) : NewValueType<CustomerId, Guid>(Value);

public record Customer(
    CustomerId Id,
    UserId UserId,
    CustomerFullName FullName,
    CustomerContact Contact,
    AddressId? DefaultDeliveryAddressId)
{
    public IEnumerable<IDomainEvent> DomainEvents { get; init; } = [];

    public static Customer New(
        UserId userId,
        CustomerFullName fullName,
        CustomerContact contact,
        AddressId? defaultDeliveryAddressId,
        TimeProvider time)
    {
        var newId = CustomerId.FromValue(Guid.NewGuid());
        var domainEvent = new CustomerCreated(newId.Value);
        return new(newId, userId, fullName, contact, defaultDeliveryAddressId)
        {
            DomainEvents = [domainEvent]
        };
    }
}

public record CustomerCreated(Guid Id) : DomainEvent;
