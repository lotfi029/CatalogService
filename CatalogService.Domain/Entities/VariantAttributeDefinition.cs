using CatalogService.Domain.DomainEvents.VariantAttribute;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.Entities;

public class VariantAttributeDefinition : AuditableEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public VariantsType Datatype { get; private set; } = default!;
    public bool AffectsInventory { get; private set; } = false;

    public ValuesJson? AllowedValues { get; private set; }

    public ICollection<CategoryVariantAttribute> CategoryVariantAttributes { get; } = [];


    private VariantAttributeDefinition() { }
    private VariantAttributeDefinition(
        string code,
        string name,
        VariantsType datatype,
        bool affectsInventory,
        ValuesJson? allowedValues
        ) 
    {
        Code = code;
        Name = name;
        Datatype = datatype;
        AffectsInventory = affectsInventory;
        AllowedValues = allowedValues;
    }

    public static Result<VariantAttributeDefinition> Create(
        string code,
        string name,
        VariantsType dataType,
        bool affectsInventory,
        ValuesJson? allowedValues
        )
    {
        if (VerifyAllowedValues(dataType, allowedValues) is { IsFailure: true } verificationError)
            return verificationError.Error;

        var variantAttribute =  new VariantAttributeDefinition(
            code: code,
            name: name,
            datatype: dataType,
            affectsInventory: affectsInventory,
            allowedValues: allowedValues);

        variantAttribute.AddDomainEvent(new VariantAttributeCreatedDomainEvent(Id: variantAttribute.Id));

        return variantAttribute;
    }

    public Result Update(string name, ValuesJson? allowedValues)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainErrors.Null(nameof(Name));

        if (VerifyAllowedValues(Datatype, allowedValues) is { IsFailure: true } verificationError)
            return verificationError.Error;

        AllowedValues = allowedValues;
        Name = name;

        AddDomainEvent(new VariantAttributeUpdatedDomainEvent(Id));
        return Result.Success();
    }
    public Result Deleted()
    {
        base.Delete();
        AddDomainEvent(new VariantAttributeDeletedDomainEvent(Id));
        return Result.Success();
    }
    private static Result VerifyAllowedValues(VariantsType dataType, ValuesJson? allowedValues)
    {
        if (dataType.DataType == VariantDataType.Select)
        {
            if (allowedValues is null)
                return DomainErrors.VariantAttributeDefinition.RequiredAllowedValues;

            if (allowedValues.Values.Count == 0)
                return DomainErrors.VariantAttributeDefinition.EmptyAllowedValues;
        }
        else
        {
            if (allowedValues is not null)
                return DomainErrors.VariantAttributeDefinition.NotApplicableAllowedValues;
        }
        return Result.Success();
    }
}