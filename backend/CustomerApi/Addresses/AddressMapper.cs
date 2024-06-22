using Riok.Mapperly.Abstractions;

namespace CustomerApi.Addresses;

[Mapper]
public partial class AddressMapper
{
    [MapNestedProperties(nameof(Address.Fields))]
    public virtual partial AddressDto MapToAddressDto(Address dto);
}
