namespace CatalogService.Domain.DomainEvents.Products.ProductVariants;

public sealed record ProductVariantDeletedAllDomainEvent(Guid Id): IDomainEvent;
