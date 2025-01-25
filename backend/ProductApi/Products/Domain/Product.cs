using Common.Domain;
using Common.Domain.ValueObjects;
using System.Text.Json;

namespace ProductApi.Products.Domain;

public class Product(ProductId id) : AggregateRoot<ProductId>(id)
{
    public ProductTitle Title { get; private set; } = ProductTitle.InitialValue;
    public ProductPrice Price { get; private set; } = ProductPrice.Zero;
    public ProductQuantity Quantity { get; private set; } = ProductQuantity.Zero;
    public bool IsActive { get; private set; } = false;

    public static Product New(
        ProductTitle title,
        ProductPrice price,
        TimeProvider timeProvider)
    {
        var productId = ProductId.NewId();

        var product = new Product(productId)
        {
            Title = title,
            Price = price,
        };

        var domainEvent = new ProductCreated(productId.Value, title.Value)
        {
            OccurredAt = timeProvider.GetLocalNow(),
        };

        product.AddDomainEvent(domainEvent);

        return product;
    }

    public void Adjust(ProductQuantity delta, TimeProvider time)
    {
        var previousQuantity = Quantity;
        var newQuantity = delta with
        {
            Value = Quantity.Value + delta.Value
        };

        Quantity = newQuantity;

        var domainEvent = new ProductAdjusted(
            Id.Value, Title.Value, Quantity.Value, previousQuantity.Value)
        {
            OccurredAt = time.GetLocalNow()
        };

        AddDomainEvent(domainEvent);
    }

    public static Product Restore(JsonDocument[] row)
    {
        var productId = ProductId.FromValue(Guid.Parse(row[0].Deserialize<string>()!));
        return new(productId)
        {
            Title = ProductTitle.FromValue(row[1].Deserialize<string>()!),
            Price = ProductPrice.FromValue(row[2].Deserialize<decimal>()!),
            Quantity = ProductQuantity.FromValue(row[4].Deserialize<int>()!),
            IsActive = row[3].Deserialize<bool>()!,
        };
    }
}
