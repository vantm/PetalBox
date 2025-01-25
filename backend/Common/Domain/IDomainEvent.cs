namespace Common.Domain;

public interface IDomainEvent
{
    Guid MessageId { get; }
    DateTimeOffset OccurredAt { get; }
}
