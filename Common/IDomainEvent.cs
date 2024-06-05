namespace Common;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTimeOffset OccurredAt { get; }
}
