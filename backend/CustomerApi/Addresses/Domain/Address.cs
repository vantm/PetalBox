namespace CustomerApi.Addresses.Domain;

public record Address(AddressId Id, CustomerId CustomerId, AddressFields Fields, UserId UserId, bool IsDefault)
{
    public IEnumerable<IDomainEvent> DomainEvents { get; init; } = [];

    public Address UpdateFields(AddressFields fields, TimeProvider time)
    {
        var domainEvent = new AddressUpdated(Id.Value, CustomerId.Value, UserId.Value)
        {
            OccurredAt = time.GetLocalNow()
        };

        return this with
        {
            Fields = fields,
            DomainEvents = [.. DomainEvents, domainEvent]
        };
    }

    public static Address New(CustomerId customerId, AddressFields fields, UserId userId, TimeProvider time)
    {
        var addressId = AddressId.FromValue(Guid.NewGuid());
        var domainEvent = new AddressCreated(addressId.Value, customerId.Value, userId.Value)
        {
            OccurredAt = time.GetLocalNow()
        };

        return new(addressId, customerId, fields, userId, false)
        {
            DomainEvents = [domainEvent]
        };
    }
}
