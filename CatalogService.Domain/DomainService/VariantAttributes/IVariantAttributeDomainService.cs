
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.VariantAttributes;

public interface IVariantAttributeDomainService
{
    Task<Result<VariantAttributeDefinition>> CreateAsync(string code, string name, string datatype, bool affectsInventory, bool affectsPricing, short diplayOrder, AllowedValuesJson? allowedValues, CancellationToken ct = default);
}