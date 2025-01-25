using Common.Domain;

namespace ProductApi.Products.Domain;

public record ProductAdjusted(Guid Id, string Title, int Quantity, int PreviousQuantity) : DomainEvent;
