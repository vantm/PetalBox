namespace Common.Domain;

public static class ServiceBusExtensions
{
    public static Eff<Unit> PublishEventsAsync(this IServiceBus serviceBus, IAggregateRoot entity) =>
        liftEff(() =>
        {
            var domainEvents = entity.GetDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                serviceBus.PublishAsync(domainEvent);
            }
            entity.ClearDomainEvents();
        });
}
