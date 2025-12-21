namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductAttributeDeletedAllDomainEvent(Guid Id) : IDomainEvent;