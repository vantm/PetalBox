namespace OrderApi.Orders.Domain;

public record SaleOrderLineId(Guid Value) : GuidId<SaleOrderLineId>(Value);
