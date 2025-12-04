using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.Attributes;

public sealed class AttributeDomainService(
    IAttributeRepository attributeRepository) : IAttributeDomainService
{
    public async Task<Result<Guid>> CreateAsync(string name, string code, string optionType, bool isFilterable, bool isSearchable, ValuesJson? options, CancellationToken ct = default)
    {
        if (await attributeRepository.ExistsAsync(e => e.Code == code, ct))
            return AttributeErrors.DuplicatedCode(code);
        
        if (!Enum.TryParse<ValuesDataType>(optionType, ignoreCase: true, out var enumOptionType))
            return DomainErrors.Attributes.InvalidCastingEnum;

        var attribute = Entities.Attribute.Create(
            name: name,
            code: code,
            optionType: new(enumOptionType),
            isFilterable: isFilterable,
            isSearchable: isSearchable,
            options: options
            );

        if (attribute.IsFailure)
            return attribute.Error;

        attributeRepository.Add(attribute.Value!);

        return attribute.Value!.Id;
    }// use excute update async
    public async Task<Result> UpdateOptionsAsync(Guid id, ValuesJson options, CancellationToken ct = default)
    {
        if (await attributeRepository.FindByIdAsync(id, ct) is not { } attribute)
            return AttributeErrors.NotFound(id);

        if (attribute.OptionsType.DataType != ValuesDataType.Select)
            return AttributeErrors.InvalidUpdateOptions;

        //await attributeRepository.ExcuteUpdateAsync(
        //    predicate: a => a.Id == id,
        //    action: x =>
        //    {
        //        x.SetProperty(e => e.Options, options);
        //    }, ct);


        attribute.UpdateOptions(options);
        attributeRepository.Update(attribute);

        return Result.Success();
    }
    public async Task<Result> UpdateDetailsAsync(
        Guid id,
        string name,
        bool isFilterable,
        bool isSearchable,
        CancellationToken ct = default)
    {
        if (await attributeRepository.FindByIdAsync(id, ct) is not { } attribute)
            return AttributeErrors.NotFound(id);

        attribute.UpdateDetails(
            name: name,
            isFilterable: isFilterable,
            isSearchable: isSearchable);

        attributeRepository.Update(attribute);

        return Result.Success();
    }
}