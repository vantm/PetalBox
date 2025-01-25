using CustomerApi.Addresses.Domain;
using CustomerApi.Addresses.Models;
using Riok.Mapperly.Abstractions;
using System.Text.Json;

namespace CustomerApi.Addresses;

[Mapper]
public partial class AddressMapper
{
    public virtual Address MapToAddress(JsonDocument[] row)
    {
        var id = AddressId.FromValue(row[0].Deserialize<Guid>());
        var customerId = CustomerId.FromValue(row[1].Deserialize<Guid>());
        var fields = new AddressFields(
            row[2].Deserialize<string>()!,
            row[3].Deserialize<string>()!,
            row[4].Deserialize<string>()!);
        var userId = UserId.FromValue(row[5].Deserialize<Guid>());
        var isDefault = row[6].Deserialize<bool>();
        return new(id, customerId, fields, userId, isDefault);
    }

    [MapNestedProperties(nameof(Address.Fields))]
    [MapperIgnoreSource(nameof(Address.DomainEvents))]
    public virtual partial AddressDto MapToAddressDto(Address dto);

    public virtual AddressDto MapToAddressDto(JsonDocument[] row)
    {
        var id = row[0].Deserialize<Guid>();
        var customerId = row[1].Deserialize<Guid>();
        var userId = row[5].Deserialize<Guid>();
        var isDefault = row[6].Deserialize<bool>();
        return new(
            id,
            customerId,
            row[2].Deserialize<string>()!,
            row[3].Deserialize<string>()!,
            row[4].Deserialize<string>()!,
            userId,
            isDefault);
    }
}
