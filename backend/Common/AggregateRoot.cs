using System.Text.Json.Serialization;

namespace Common;

public abstract class AggregateRoot<T> : IAggregateRoot
{
    public T Id { get; protected set; } = default!;

    [JsonIgnore]
    private readonly IList<IDomainEvent> _events = [];

    protected void AddDomainEvent(IDomainEvent evt)
    {
        _events.Add(evt);
    }

    public IEnumerable<IDomainEvent> GetDomainEvents()
    {
        return [.. _events];
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}
