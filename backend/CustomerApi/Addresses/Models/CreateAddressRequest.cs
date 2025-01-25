namespace CustomerApi.Addresses.Models;

public record CreateAddressRequest(
    string AddressName,
    string AddressText,
    string Phone);
