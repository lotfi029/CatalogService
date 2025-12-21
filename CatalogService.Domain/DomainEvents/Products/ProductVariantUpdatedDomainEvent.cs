namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductVariantUpdatedDomainEvent(Guid Id, Guid VaraintId): IDomainEvent;
