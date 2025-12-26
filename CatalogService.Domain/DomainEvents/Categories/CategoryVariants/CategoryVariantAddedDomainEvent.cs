namespace CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

public sealed record CategoryVariantAddedDomainEvent(Guid Id, Guid VariantAttributeId) : IDomainEvent;
