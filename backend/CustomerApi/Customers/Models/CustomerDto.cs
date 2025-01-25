namespace CustomerApi.Customers.Models;

public record CustomerDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Phone);
