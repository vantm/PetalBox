namespace CustomerApi.Addresses.Models;

public record UpdateAddressRequest(
    string AddressName,
    string AddressText,
    string Phone);
