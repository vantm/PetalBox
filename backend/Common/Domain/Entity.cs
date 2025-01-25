namespace Common.Domain;

public abstract class Entity<T>
{
    protected Entity(T id)
    {
        Id = id;
        _hashCode = HashCode.Combine(GetType().FullName, id);
    }

    public T Id { get; }

    private readonly int _hashCode;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (GetType() != obj.GetType()) return false;
        return GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode() => _hashCode;
}
