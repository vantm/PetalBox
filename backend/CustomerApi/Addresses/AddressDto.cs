namespace CustomerApi.Addresses;

public record AddressDto(
    Guid Id,
    Guid UserId,
    string AddressText,
    string? City,
    string? State,
    string? PostalCode,
    string? Country);
