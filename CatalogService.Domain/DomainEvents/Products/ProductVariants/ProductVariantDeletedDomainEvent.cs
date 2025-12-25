namespace CatalogService.Domain.DomainEvents.Products.ProductVariants;

public sealed record ProductVariantDeletedDomainEvent(Guid Id, Guid VariantId): IDomainEvent;