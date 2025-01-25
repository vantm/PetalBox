namespace CustomerApi.Addresses.Models;

public record AddressDto(
    Guid Id,
    Guid CustomerId,
    string AddressName,
    string AddressText,
    string Phone,
    Guid UserId,
    bool IsDefault);
