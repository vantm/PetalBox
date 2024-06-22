namespace Common;

public record UserId : GuidId<UserId>
{
    private UserId(Guid value) : base(value)
    {
    }
}
