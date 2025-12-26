namespace CatalogService.Domain.DomainEvents.Categories;

public sealed record CategoryCreatedDomainEvent(Guid Id) : IDomainEvent;
