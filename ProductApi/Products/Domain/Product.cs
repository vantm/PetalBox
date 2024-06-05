using System.Text.Json;

namespace ProductApi.Products.Domain;

public class Product : AggregateRoot<Guid>
{
    private Product() { }

    public string Title { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } = false;
    public int Quantity { get; private set; } = 0;

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

    public void Adjust(int delta)
    {
        var previousQuantity = Quantity;

        Quantity += delta;

        AddDomainEvent(new ProductAdjusted(Id, Title, Quantity, previousQuantity));
    }

    public static Product New(string title, decimal price, TimeProvider timeProvider)
    {
        var entity = new Product()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Price = price,
        };

        entity.AddDomainEvent(new ProductCreated(entity.Id, entity.Title)
        {
            OccurredAt = timeProvider.GetLocalNow(),
        });

        return entity;
    }
}
