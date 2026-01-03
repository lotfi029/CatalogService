namespace CatalogService.Domain.DomainEvents.Products.ProductVariants;

public sealed record ProductVariantAddedDomainEvent(
    Guid Id, 
    Guid VariantId,
    Guid[] VariantAttributeIds): IDomainEvent;