using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.VariantAttributes;

public interface IVariantAttributeDomainService
{
    Task<Result<VariantAttributeDefinition>> CreateAsync(string code, string name, string datatype, bool affectsInventory, AllowedValuesJson? allowedValues, CancellationToken ct = default);
    Task<Result> CreateBulkAsync(IEnumerable<(string code, string name, string datatype, bool affectsInventory, AllowedValuesJson? allowedValues)> variantAttributes, CancellationToken ct = default);
}