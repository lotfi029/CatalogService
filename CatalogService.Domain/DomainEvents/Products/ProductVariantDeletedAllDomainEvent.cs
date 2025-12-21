namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductVariantDeletedAllDomainEvent(Guid Id): IDomainEvent;
