namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductVariantAddedDomainEvent(Guid Id, Guid VariantId): IDomainEvent;