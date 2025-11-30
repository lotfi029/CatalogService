using CatalogService.Domain.Errors;
using CatalogService.Domain.IRepositories;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.VariantAttributes;

public sealed class VariantAttributeDomainService(IVariantAttributeRepository variantAttributeRepository) : IVariantAttributeDomainService
{
    public async Task<Result<VariantAttributeDefinition>> CreateAsync(
        string code,
        string name,
        string datatype,
        bool affectsInventory,
        bool affectsPricing,
        short diplayOrder,
        AllowedValuesJson? allowedValues,
        CancellationToken ct = default)
    {
        if (await variantAttributeRepository.ExistsAsync(e => e.Code == code, ct))
            return VariantAttributeErrors.CodeAlreadyExist(code);
        if (!Enum.TryParse<VaraintAttributeDatatype>(datatype, ignoreCase: true, out var enumDataType))
            throw new ArgumentException("Must specify the datatype of the variant attribute definition");

        var variantAttribute = VariantAttributeDefinition.Create(
            code: code,
            name: name,
            datatype: enumDataType,
            affectsInventory: affectsInventory,
            affectsPricing: affectsPricing,
            diplayOrder: diplayOrder,
            allowedValues: allowedValues);

        return variantAttribute;

    }
}
