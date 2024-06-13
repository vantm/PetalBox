using System.Text.Json;

namespace ProductApi.Products.Domain;

public record Product
{
    public required Guid Id { get; init; }
    public required string Title { get; init; } = string.Empty;
    public required decimal Price { get; init; }
    public bool IsActive { get; init; } = false;
    public int Quantity { get; init; } = 0;

    public Seq<IDomainEvent> DomainEvents { get; init; }

    public static Product New(string title, decimal price, TimeProvider timeProvider)
    {
        var id = Guid.NewGuid();
        return new Product()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Price = price,
            DomainEvents =
            [
                new ProductCreated(id, title)
                {
                    OccurredAt = timeProvider.GetLocalNow(),
                }
            ]
        };
    }

    public Product Adjust(int delta, TimeProvider time)
    {
        var previousQuantity = Quantity;
        var newQuantity = Quantity + delta;

        var domainEvent = new ProductAdjusted(Id, Title, Quantity, previousQuantity)
        {
            OccurredAt = time.GetLocalNow()
        };

        return this with
        {
            Quantity = newQuantity,
            DomainEvents = [.. DomainEvents, domainEvent]
        };
    }

    public static Product FromJsonArray(JsonDocument[] row)
    {
        return new()
        {
            Id = Guid.Parse(row[0].Deserialize<string>()!),
            Title = row[1].Deserialize<string>()!,
            Price = row[2].Deserialize<decimal>()!,
            IsActive = row[3].Deserialize<bool>()!,
            Quantity = row[4].Deserialize<int>()!,
        };
    }
}
