namespace CustomerApi.Addresses;

public record AddressFields(
    string AddressText,
    string? City,
    string? State,
    string? PostalCode,
    string? Country);
