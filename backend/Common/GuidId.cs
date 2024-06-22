namespace Common;

public abstract record GuidId<TIdType>(Guid Value) : NewValueType<TIdType, Guid>(Value)
    where TIdType : GuidId<TIdType>
{
    public static TIdType NewId()
        => Activator.CreateInstance<TIdType>();
}
