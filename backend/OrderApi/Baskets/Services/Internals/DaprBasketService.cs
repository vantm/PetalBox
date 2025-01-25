using Common.Domain;
using Common.Domain.ValueObjects;
using Dapr.Client;
using Microsoft.Extensions.Options;
using OrderApi.Baskets.Domain;

namespace OrderApi.Baskets.Services.Internals;

public class DaprBasketService(
    DaprClient dapr,
    IOptions<CommonOptions> Options,
    IServiceBus serviceBus,
    TimeProvider timeProvider) : IBasketService
{
    private readonly string _stateStoreName = Options.Value.StateStoreName;
    public Task<Basket?> GetAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var stateStoreKey = BasketStateStoreKey(userId);
        return dapr.GetStateAsync<Basket?>(
            _stateStoreName, stateStoreKey, cancellationToken: cancellationToken);
    }

    public async Task SaveAsync(Basket basket, CancellationToken cancellationToken = default)
    {
        var stateStoreKey = BasketStateStoreKey(basket.UserId);

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

    public string BasketStateStoreKey(UserId userId) =>
        $"basket-module.user.{userId}.basket";
}
