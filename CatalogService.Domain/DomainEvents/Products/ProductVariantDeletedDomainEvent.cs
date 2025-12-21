namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductVariantDeletedDomainEvent(Guid Id, Guid VaraintId): IDomainEvent;