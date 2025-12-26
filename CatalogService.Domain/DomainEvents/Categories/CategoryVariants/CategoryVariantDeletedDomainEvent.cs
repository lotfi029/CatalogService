namespace CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

public sealed record CategoryVariantDeletedDomainEvent(Guid Id) : IDomainEvent;