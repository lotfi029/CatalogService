using CatalogService.Application.DTOs.CategoryVariantAttributes;

namespace CatalogService.Application.DTOs.Categories;
public record CategoryDetailedResponse(
    Guid Id,
    string Name,
    string Slug,
    Guid? ParentId,
    short Level,
    string? Description,
    string? Path,
    List<CategoryVariantForCategoryResponse>? Variants = null
    )
{
    private CategoryDetailedResponse()
        : this(Guid.Empty, string.Empty, string.Empty, null, 0, null, null){}
    
};