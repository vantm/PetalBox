namespace Common.Domain.ValueObjects;

public record SaleOrderLineId(Guid Value) : GuidId<SaleOrderLineId>(Value);
