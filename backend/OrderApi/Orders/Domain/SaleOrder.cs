namespace OrderApi.Orders.Domain;

public record SaleOrder(
    SaleOrderId Id,
    UserId UserId,
    DateTimeOffset OrderAt,
    SaleOrderStatus Status,
    decimal OrderTotal,
    IEnumerable<SaleOrderLine> Lines)
{
    public IEnumerable<IDomainEvent> DomainEvents { get; init; } = [];

    public static SaleOrder New(UserId userId, TimeProvider time, IEnumerable<SaleOrderLine> lines)
    {
        var total = lines.Sum(x => x.Price * x.Quantity);
        var orderId = SaleOrderId.NewId();
        var order = new SaleOrder(orderId, userId, time.GetLocalNow(), SaleOrderStatus.New, total, lines)
        {
            DomainEvents = [new SaleOrderCreated(orderId.Value, userId.Value)]
        };

        return order;
    }

    public SaleOrder ClearEvents() => this with { DomainEvents = [] };
}
