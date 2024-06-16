namespace Common;

public interface IDomainEvent
{
    Guid MessageId { get; }
    DateTimeOffset OccurredAt { get; }
}
