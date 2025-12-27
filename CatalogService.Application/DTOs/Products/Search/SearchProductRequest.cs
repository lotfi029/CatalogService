namespace CatalogService.Application.DTOs.Products.Search;

public sealed record SearchProductRequest(
    string? SearchTerm,
    List<Guid>? CategoryIds,
    Dictionary<string, List<string>>? Filters,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page,
    int Size
    );