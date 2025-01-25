using Common.Domain.ValueObjects;

namespace OrderApi.Baskets.Domain;

public sealed record BasketId(Guid id) : GuidId<BasketId>(id)
{
}
