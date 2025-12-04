using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.VariantAttributes;

public interface IVariantAttributeDomainService
{
    Task<Result<VariantAttributeDefinition>> CreateAsync(string code, string name, string datatype, bool affectsInventory, ValuesJson? allowedValues, CancellationToken ct = default);
    Task<Result> CreateBulkAsync(IEnumerable<(string code, string name, string datatype, bool affectsInventory, ValuesJson? allowedValues)> variantAttributes, CancellationToken ct = default);
}