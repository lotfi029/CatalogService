namespace CatalogService.Domain.DomainService.ProductVariantValue;

public sealed record ProductVariantValueCreatedDomainEvent(Guid Id, Guid ProductId, Guid VariantAttributeId) : IDomainEvent;