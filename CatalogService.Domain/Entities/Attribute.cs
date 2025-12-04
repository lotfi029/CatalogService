using CatalogService.Domain.DomainEvents.Attributes;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.Entities;

public class Attribute : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Code { get; } = string.Empty;
    public OptionsType OptionsType { get; } = default!;

    public bool IsFilterable { get; private set; } = false;
    public bool IsSearchable { get; private set; } = false;

    public ValuesJson? Options { get; private set; }
    private Attribute() { }
    private Attribute(
        string name, 
        string code,
        OptionsType type,
        bool isFilterable = false, 
        bool isSearchable = false,
        ValuesJson? options = null
        )
    {
        Name = name;
        Code = code;
        OptionsType = type;
        IsFilterable = isFilterable;
        IsSearchable = isSearchable;
        Options = options;

        AddDomainEvent(new AttributeCreatedDomainEvent(Id));
    }

    public static Result<Attribute> Create(
        string name, 
        string code,
        OptionsType optionType, 
        bool isFilterable,
        bool isSearchable,
        ValuesJson? options
        )
    {
        if (VerifyOptions(optionType, options) is { IsFailure: true } result)
            return result.Error;

        if (optionType.DataType == ValuesDataType.UnAssign)
            return DomainErrors.Attributes.InvalidOptionType;

        return new Attribute(
            name,
            code,
            type: optionType,
            isFilterable,
            isSearchable,
            options
            );
    }
    public Result UpdateDetails(string name, bool isFilterable, bool isSearchable)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainErrors.Null(name);

        
        Name = name;
        IsFilterable = isFilterable;
        IsSearchable = isSearchable;

        AddDomainEvent(new AttributeDetailsUpdatedDomainEvent(Id));

        return Result.Success();
    }
    public Result UpdateOptions(ValuesJson options)
    {
        if (VerifyOptions(OptionsType, options) is { IsFailure: true} result)
            return result;

        Options = options;
        AddDomainEvent(new AttributeOptionsUpdatedDomainEvent(Id));
        return Result.Success();
    }
    public Result Activate()
    {
        if (IsActive)
            return DomainErrors.Attributes.InvalidActiveOperation;

        Active();

        AddDomainEvent(new AttributeActivatedDomainEvent(Id));
        return Result.Success();
    }
    public Result Deactivate()
    {
        if (!IsActive)
            return DomainErrors.Attributes.InvalidDeactiveOperation;
        Deactive();
        AddDomainEvent(new AttributeDeactivatedDomainEvent(Id));
        return Result.Success();
    }
    public Result Deleted()
    {
        if (IsDeleted)
            return DomainErrors.Attributes.NotFound;

        Delete();

        AddDomainEvent(new AttributeDeletedDomainEvent(Id));
        return Result.Success();
    }
    private static Result VerifyOptions(OptionsType datatype, ValuesJson? options)
    {

        if (datatype.DataType == ValuesDataType.Select)
        {
            if (options is null)
                return DomainErrors.Attributes.NullOptions;

            var values = options.Values;

            if (values.Count == 0)
                return DomainErrors.Attributes.EmptyOptions;

            if (values.Count != values.Distinct().Count())
                return DomainErrors.Attributes.DuplicateOptions;

            if (values.Count > 50)
                return DomainErrors.Attributes.OutOfRangeOptions(50);

        }
        else
        {
            if (options is not null)
                return DomainErrors.Attributes.InvalidOptions;
        }
        return Result.Success();
    }
}
