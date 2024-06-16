namespace Common;

public abstract record DomainEvent : IDomainEvent
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.Now;
}
