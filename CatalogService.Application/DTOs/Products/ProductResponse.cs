namespace CatalogService.Application.DTOs.Products;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    Guid VendorId)
{
    private ProductResponse() : this(
        Id: Guid.Empty, 
        Name: string.Empty,
        Description: null, 
        Status: string.Empty,
        VendorId: Guid.Empty){}
}
