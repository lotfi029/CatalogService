using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.VariantAttributes;

public sealed class VariantAttributeDomainService(IVariantAttributeRepository variantAttributeRepository) : IVariantAttributeDomainService
{
    public async Task<Result<VariantAttributeDefinition>> CreateAsync(
        string code,
        string name,
        string datatype,
        bool affectsInventory,
        ValuesJson? allowedValues,
        CancellationToken ct = default)
    {
        if (await variantAttributeRepository.ExistsAsync(e => e.Code == code, ct))
            return VariantAttributeErrors.CodeAlreadyExist(code);
        
        if (!Enum.TryParse<ValuesDataType>(datatype, ignoreCase: true, out var enumDataType))
            throw new ArgumentException("Must specify the datatype of the variant attribute definition");

        var variantAttribute = VariantAttributeDefinition.Create(
            code: code,
            name: name,
            dataType: new(enumDataType),
            affectsInventory: affectsInventory,
            allowedValues: allowedValues);

        variantAttributeRepository.Add(variantAttribute);

        return variantAttribute;
    }
    public async Task<Result> CreateBulkAsync(
        IEnumerable<(string code, string name, string datatype, bool affectsInventory, ValuesJson? allowedValues)> variantAttributes,
        CancellationToken ct = default
        )
    {

        foreach (var (code, name, datatype, affectsInventory, allowedValues) in variantAttributes)
        {
            var result = await CreateAsync(code, name, datatype, affectsInventory, allowedValues, ct);
            
            if (result.IsFailure) 
                return result;
        }
        return Result.Success();
    }
}
