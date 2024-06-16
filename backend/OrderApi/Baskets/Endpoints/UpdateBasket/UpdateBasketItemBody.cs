namespace OrderApi.Baskets.Endpoints.UpdateBasket;

public record UpdateBasketItemBody(Guid ProductId, int Quantity);
