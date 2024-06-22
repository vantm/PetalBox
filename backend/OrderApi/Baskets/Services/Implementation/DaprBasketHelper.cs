namespace OrderApi.Baskets.Services.Implementation;

public class DaprBasketHelper
{
    public string BasketStateStoreKey(UserId userId) =>
        $"basket-module.user.{userId}.basket";
}
