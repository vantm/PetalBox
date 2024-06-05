namespace Common;

public interface IServiceBus
{
    Task PublishAsync(object evt);
}
