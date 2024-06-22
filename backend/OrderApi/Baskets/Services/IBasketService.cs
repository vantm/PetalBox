using OrderApi.Baskets.Domain;

namespace OrderApi.Baskets.Services;

public interface IBasketService
{
    Task<Basket?> GetAsync(UserId userId, CancellationToken cancellationToken = default);
    Task SaveAsync(Basket basket, CancellationToken cancellationToken = default);
}