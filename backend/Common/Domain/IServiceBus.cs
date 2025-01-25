namespace Common.Domain;

public interface IServiceBus
{
    ValueTask PublishAsync(IDomainEvent evt);
}
