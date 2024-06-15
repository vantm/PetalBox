namespace OrderApi.Baskets.Models;

public record CartDto(Guid Id, Guid UserId, IEnumerable<CartItemDto> Items);
