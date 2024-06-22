namespace CustomerApi.Addresses;

public record Address(AddressId Id, UserId UserId, AddressFields Fields)
{
    public IEnumerable<IDomainEvent> DomainEvents { get; init; } = [];

    public static Address New(UserId userId, AddressFields fields, TimeProvider time)
    {
        var addressId = AddressId.FromValue(Guid.NewGuid());
        var domainEvent = new AddressCreated(addressId.Value, userId.Value)
        {
            OccurredAt = time.GetLocalNow()
        };
        return new(addressId, userId, fields)
        {
            DomainEvents = [domainEvent]
        };
    }
}
