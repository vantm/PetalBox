namespace Common;

public record ProductId : GuidId<ProductId>
{
    private ProductId(Guid value) : base(value)
    {
    }
}