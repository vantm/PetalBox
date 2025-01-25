namespace Common.Domain.ValueObjects;

#pragma warning disable S3453 // Classes should not have only "private" constructors
public sealed record UserId : GuidId<UserId>
#pragma warning restore S3453 // Classes should not have only "private" constructors
{
#pragma warning disable IDE0051 // Remove unused private members
    private UserId(Guid value) : base(value)
#pragma warning restore IDE0051 // Remove unused private members
    {
    }
}
