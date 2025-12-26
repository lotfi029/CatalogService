namespace CatalogService.Domain.DomainEvents.Categories;

public sealed record CategoryDeletedDomainEvent(Guid Id) : IDomainEvent;
