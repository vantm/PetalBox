using Common.Domain;
using Common.Domain.ValueObjects;

namespace OrderApi.Orders.Domain;

public class SaleOrder : AggregateRoot<SaleOrderId>
{
    private SaleOrder(SaleOrderId id) : base(id)
    {
    }

    public UserId UserId { get; private set; }
    public DateTimeOffset OrderAt { get; private set; }
    public SaleOrderStatus Status { get; private set; }
    public decimal OrderTotal { get; private set; }
    public IEnumerable<SaleOrderLine> Lines { get; private set; } = [];

    public static SaleOrder New(UserId userId, TimeProvider time, IEnumerable<SaleOrderLine> lines)
    {
        var total = lines.Sum(x => x.Price * x.Quantity);

        var orderId = SaleOrderId.NewId();

        var order = new SaleOrder(orderId)
        {

            UserId = userId,
            OrderAt = time.GetLocalNow(),
            Status = SaleOrderStatus.New,
            OrderTotal = total,
            Lines = lines
        };

        var domainEvent = new SaleOrderCreated(orderId.Value, userId.Value);

        order.AddDomainEvent(domainEvent);

        return order;
    }
}
