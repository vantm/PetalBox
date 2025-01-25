namespace Common.Domain.ValueObjects;

#pragma warning disable S3453 // Classes should not have only "private" constructors
public record SaleOrderId : GuidId<SaleOrderId>
#pragma warning restore S3453 // Classes should not have only "private" constructors
{
#pragma warning disable IDE0051 // Remove unused private members
    private SaleOrderId(Guid Value) : base(Value)
#pragma warning restore IDE0051 // Remove unused private members
    {
    }
}
