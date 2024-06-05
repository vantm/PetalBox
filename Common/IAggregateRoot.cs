namespace Common;

public interface IAggregateRoot
{
    IEnumerable<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
