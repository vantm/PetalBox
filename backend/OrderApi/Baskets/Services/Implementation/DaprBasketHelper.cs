namespace OrderApi.Baskets.Services.Implementation;

public class DaprBasketHelper
{
    public string BasketStateStoreKey(Guid userId) =>
        $"basket-module.user.{userId}.basket";
}
