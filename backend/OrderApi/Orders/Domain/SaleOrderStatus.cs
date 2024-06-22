namespace OrderApi.Orders.Domain;

public class SaleOrderStatus : SmartEnum<SaleOrderStatus>
{
    private SaleOrderStatus(string name, int value) : base(name, value)
    {
    }

    public static readonly SaleOrderStatus Pending = new("Pending", 0);
    public static readonly SaleOrderStatus New = new("New", 1000);
    public static readonly SaleOrderStatus InProgress = new("InProgress", 2000);
    public static readonly SaleOrderStatus Completed = new("Completed", 3000);
    public static readonly SaleOrderStatus Error = new("Error", 4000);
    public static readonly SaleOrderStatus Canceled = new("Canceled", 5000);
}
