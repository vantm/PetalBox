namespace OrderApi.Baskets.Domain;

public class BasketId : NewType<BasketId, Guid>
{
    private BasketId(Guid value) : base(value)
    {
    }

    public static BasketId NewId() => new(Guid.NewGuid());

    public static BasketId FromGuid(Guid value) => new(value);
}
