namespace Common;

public static class ServiceBusExtensions
{
    public static Task<Either<Error, Unit>> PublishEventsAsync(this IServiceBus serviceBus, IAggregateRoot entity)
        => Some(entity).ToEither(BottomError.Default)
        .LeftWhen(isDefault, () => Error.New("Entity.Null"))
        .BindAsync(entity =>
        {
            var events = entity.GetDomainEvents();
            return serviceBus.PublishEventsAsync(events);
        })
        .Tap(x => x.Tap(_ =>
        {
            entity.ClearDomainEvents();
        }));

    public static Task<Either<Error, Unit>> PublishEventsAsync(this IServiceBus serviceBus, IEnumerable<IDomainEvent> @events)
        => Some(events).ToEither(BottomError.Default)
        .LeftWhen(isDefault, () => Error.New("Entity.Null"))
        .BindAsync(async events =>
        {
            try
            {
                await Parallel.ForEachAsync(events, async (e, _) => await serviceBus.PublishAsync(e));
                return EitherWithError(unit);
            }
            catch (Exception ex)
            {
                return Error.New("PublishEvents.Error", ex);
            }
        });
}
