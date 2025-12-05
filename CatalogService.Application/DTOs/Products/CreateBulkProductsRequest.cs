namespace CatalogService.Application.DTOs.Products;

public sealed record CreateBulkProductsRequest(List<ProductRequest> Products);
