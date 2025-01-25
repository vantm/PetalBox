namespace Common.Domain.ValueObjects;

public record AddressId(Guid Value) : NewValueType<AddressId, Guid>(Value);