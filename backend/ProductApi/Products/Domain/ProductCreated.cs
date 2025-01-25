using Common.Domain;

namespace ProductApi.Products.Domain;

public record ProductCreated(Guid Id, string Title) : DomainEvent;
