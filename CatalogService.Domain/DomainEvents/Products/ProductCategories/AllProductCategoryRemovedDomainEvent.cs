namespace CatalogService.Domain.DomainEvents.Products.ProductCategories;

public sealed record AllProductCategoryRemovedDomainEvent(Guid Id) : IDomainEvent;
