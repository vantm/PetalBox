namespace Common;

public static class ServiceBusExtensions
{
    public static Task<Either<Error, Unit>> PublishEventsAsync(this IServiceBus serviceBus, IAggregateRoot entity)
        => Some(entity).ToEither(BottomError.Default)
        .LeftWhen(isDefault, () => Error.New("Entity.Null"))
        .Map(e => e.GetDomainEvents())
        .BindAsync(async events =>
        {
            try
            {
                await Parallel.ForEachAsync(events, async (e, _) => await serviceBus.PublishAsync(e));
                entity.ClearDomainEvents();
                return EitherWithError(unit);
            }
            catch (Exception ex)
            {
                return Error.New("PublishEvents.Error", ex);
            }
        });
}
