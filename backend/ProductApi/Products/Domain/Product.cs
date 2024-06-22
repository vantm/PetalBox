using Riok.Mapperly.Abstractions;
using System.Text.Json;

namespace ProductApi.Products.Domain;

public record Product
{
    public required ProductId Id { get; init; }
    public required ProductTitle Title { get; init; }
    public required ProductPrice Price { get; init; }
    public ProductQuantity Quantity { get; init; } = new(0);
    public bool IsActive { get; init; } = false;

    [MapperIgnore]
    public Seq<IDomainEvent> DomainEvents { get; init; }

    public static Product New(
        ProductTitle title,
        ProductPrice price,
        TimeProvider timeProvider)
    {
        var id = Guid.NewGuid();
        return new Product()
        {
            Id = ProductId.NewId(),
            Title = title,
            Price = price,
            DomainEvents =
            [
                new ProductCreated(id, title.Value)
                {
                    OccurredAt = timeProvider.GetLocalNow(),
                }
            ]
        };
    }

    public Product Adjust(ProductQuantity delta, TimeProvider time)
    {
        var previousQuantity = Quantity;
        var newQuantity = delta with
        {
            Value = Quantity.Value + delta.Value
        };

        var domainEvent = new ProductAdjusted(
            Id.Value, Title.Value, Quantity.Value, previousQuantity.Value)
        {
            OccurredAt = time.GetLocalNow()
        };

        return this with
        {
            Quantity = newQuantity,
            DomainEvents = [.. DomainEvents, domainEvent]
        };
    }

    public static Product Restore(JsonDocument[] row)
    {
        return new()
        {
            Id = ProductId.FromValue(Guid.Parse(row[0].Deserialize<string>()!)),
            Title = new(row[1].Deserialize<string>()!),
            Price = new(row[2].Deserialize<decimal>()!),
            Quantity = new(row[4].Deserialize<int>()!),
            IsActive = row[3].Deserialize<bool>()!,
        };
    }
}
