namespace CustomerApi.Customers;

public record CustomerDto(
    string Id,
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string? DefaultDeliveryAddressId);
