using CatalogService.Application.DTOs.CategoryVariantAttributes;

namespace CatalogService.Application.DTOs.Categories;
public record CategoryDetailedResponse(
    Guid Id,
    string Name,
    string Slug,
    Guid? ParentId,
    short Level,
    bool IsActive,
    string? Description,
    string? Path,
    List<CategoryVariantForCategoryResponse>? Variants = null
    )
{
    private CategoryDetailedResponse() 
        : this(
            Id: Guid.Empty, 
            Name: string.Empty, 
            Slug: string.Empty, 
            ParentId: null, 
            Level: 0,
            IsActive: false, 
            Description: null,
            Path: null) { }
    
};