namespace Common.Domain.ValueObjects;

public record CustomerId(Guid Value) : NewValueType<CustomerId, Guid>(Value);
