using Common.Domain;
using Common.Domain.ValueObjects;

namespace OrderApi.Orders.Domain;

public class SaleOrderLine : Entity<SaleOrderLineId>
{
    private SaleOrderLine(SaleOrderLineId id) : base(id)
    {
    }

    public ProductId ProductId { get; private set; }
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public static SaleOrderLine Create(ProductId productId, string name, int quantity, decimal price)
    {
        var lineId = SaleOrderLineId.NewId();
        var line = new SaleOrderLine(lineId)
        {
            Name = name,
            Quantity = quantity,
            Price = price
        };
        return line;
    }
}
