namespace Common;

public interface IServiceBus
{
    ValueTask PublishAsync(IDomainEvent evt);
}
