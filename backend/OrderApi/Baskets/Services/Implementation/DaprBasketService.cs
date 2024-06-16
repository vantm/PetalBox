using Dapr.Client;
using Microsoft.Extensions.Options;
using OrderApi.Baskets.Domain;

namespace OrderApi.Baskets.Services.Implementation;

public class DaprBasketService(
    DaprClient dapr,
    IOptions<CommonOptions> Options,
    IServiceBus serviceBus,
    DaprBasketHelper helper,
    TimeProvider timeProvider) : IBasketService
{
    private readonly string _stateStoreName = Options.Value.StateStoreName;
    public Task<Basket?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var stateStoreKey = helper.BasketStateStoreKey(userId);
        return dapr.GetStateAsync<Basket?>(
            _stateStoreName, stateStoreKey, cancellationToken: cancellationToken);
    }

    public async Task SaveAsync(Basket basket, CancellationToken cancellationToken = default)
    {
        var stateStoreKey = helper.BasketStateStoreKey(basket.UserId);

        var metadata = new Dictionary<string, string>()
        {
            ["ttlInSeconds"] = "86400" // 1 day
        };

        await dapr.SaveStateAsync(
            _stateStoreName, stateStoreKey, basket,
            metadata: metadata, cancellationToken: cancellationToken);

        var domainEvent = new BasketUpdated(basket.Id, basket.UserId)
        {
            OccurredAt = timeProvider.GetLocalNow()
        };

        await serviceBus.PublishAsync(domainEvent);
    }
}
