namespace CatalogService.Domain.DomainEvents.Products.ProductVariants;

public sealed record ProductVariantUpdatedDomainEvent(Guid Id, Guid VariantId): IDomainEvent;
