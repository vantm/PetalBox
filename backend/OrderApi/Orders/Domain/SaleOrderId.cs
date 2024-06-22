namespace OrderApi.Orders.Domain;

public record SaleOrderId : GuidId<SaleOrderId>
{
    private SaleOrderId(Guid Value) : base(Value)
    {
    }
}
