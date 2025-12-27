namespace CatalogService.Application.DTOs.Products.Search;

internal sealed class SearchProductRequestValidator : AbstractValidator<SearchProductRequest>
{
    public SearchProductRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize must be between 1 and 100");

        RuleFor(x => x.Filters)
            .Must(filters => filters == null || filters.Count <= 50)
            .WithMessage("Cannot have more than 50 filter keys");

        RuleFor(x => x.Filters)
            .Must(filters => filters == null || filters.All(f =>
                !string.IsNullOrWhiteSpace(f.Key) && f.Value?.Any() == true))
            .When(x => x.Filters?.Count > 0)
            .WithMessage("Filter keys cannot be empty and must have at least one value");

        RuleFor(x => x.Filters)
            .Must(filters => filters == null || filters.All(f => f.Value.Count <= 100))
            .When(x => x.Filters?.Count > 0)
            .WithMessage("Each filter cannot have more than 100 values");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue)
            .WithMessage("MinPrice must be greater than or equal to 0");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPrice.HasValue)
            .WithMessage("MaxPrice must be greater than or equal to 0");

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("MinPrice must be less than or equal to MaxPrice");

        RuleFor(x => x.CategoryIds)
            .Must(ids => ids == null || ids.Count <= 10)
            .WithMessage("Cannot filter by more than 10 categories");
    }
}