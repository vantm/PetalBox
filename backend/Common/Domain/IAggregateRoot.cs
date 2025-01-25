namespace Common.Domain;

public interface IAggregateRoot
{
    IEnumerable<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
