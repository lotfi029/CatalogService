namespace CatalogService.Domain.DomainEvents.Categories;

public sealed record CategoryDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
