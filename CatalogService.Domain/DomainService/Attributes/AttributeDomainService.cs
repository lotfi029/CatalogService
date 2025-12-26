using CatalogService.Domain.Contants;
using CatalogService.Domain.DomainEvents.Attributes;
using CatalogService.Domain.JsonProperties;
using Attribute = CatalogService.Domain.Entities.Attribute;

namespace CatalogService.Domain.DomainService.Attributes;

public sealed class AttributeDomainService(
    IAttributeRepository attributeRepository,
    IProductAttributeRepository productAttributeRepository) : IAttributeDomainService
{
    public async Task<Result<Guid>> CreateAsync(string name, string code, string optionType, bool isFilterable, bool isSearchable, ValuesJson? options, CancellationToken ct = default)
    {
        if (await attributeRepository.ExistsAsync(e => e.Code == code, [QueryFilterConsts.SoftDeleteFilter], ct))
            return AttributeErrors.DuplicatedCode(code);

        if (!Enum.TryParse<VariantDataType>(optionType, ignoreCase: true, out var enumOptionType))
            return DomainErrors.Attributes.InvalidCastingEnum;

        var attribute = Attribute.Create(
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
    }
    public async Task<Result> UpdateOptionsAsync(Guid id, ValuesJson options, CancellationToken ct = default)
    {
        var updatedRows = await attributeRepository.ExcuteUpdateAsync(
            predicate: a => a.Id == id && a.OptionsType.DataType == VariantDataType.Select,
            action: x =>
            {
                x.SetProperty(e => e.Options, options);
            }, ct);

        if (updatedRows == 0)
            return AttributeErrors.NotFound(id);

        AddDomainEvents(id, new AttributeOptionsUpdatedDomainEvent(id));
        return Result.Success();
    }
    public async Task<Result> UpdateDetailsAsync(
        Guid id,
        string name,
        bool isFilterable,
        bool isSearchable,
        CancellationToken ct = default)
    {
        var updatedRows = await attributeRepository.ExcuteUpdateAsync(
            predicate: a => a.Id == id,
            action: x =>
            {
                x.SetProperty(e => e.Name, name);
                x.SetProperty(e => e.IsSearchable, isSearchable);
                x.SetProperty(e => e.IsFilterable, isFilterable);
            }, ct);

        if (updatedRows == 0)
            return AttributeErrors.NotFound(id);

        AddDomainEvents(id, new AttributeDetailsUpdatedDomainEvent(id));

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (await attributeRepository.FindAsync(id, null, ct) is not { } attribute)
            return AttributeErrors.NotFound(id);

        //if (attribute.Deleted() is { IsFailure: true } error)
        //    return error;

        await productAttributeRepository.ExecuteUpdateAsync(
            predicate: pa => pa.AttributeId == id,
            action: pa =>
            {
                pa.SetProperty(e => e.IsDeleted, true);
            }, ct);

        attributeRepository.Remove(attribute);

        return Result.Success();
    }
    public async Task<Result> DeactiveAsync(Guid id, CancellationToken ct = default)
    {
        var updatedRows = await attributeRepository.ExcuteUpdateAsync(
            predicate: a => a.Id == id,
            action: x =>
            {
                x.SetProperty(e => e.IsActive, false);
            }, ct);

        if (updatedRows == 0)
            return AttributeErrors.NotFound(id);

        AddDomainEvents(id, new AttributeDeactivatedDomainEvent(id));

        return Result.Success();
    }
    public async Task<Result> ActiveAsync(Guid id, CancellationToken ct = default)
    {
        var updatedRows = await attributeRepository.ExcuteUpdateAsync(
            predicate: a => a.Id == id,
            action: x =>
            {
                x.SetProperty(e => e.IsActive, true);
            }, ct);

        if (updatedRows == 0)
            return AttributeErrors.NotFound(id);

        AddDomainEvents(id, new AttributeActivatedDomainEvent(id));

        return Result.Success();
    }
    private void AddDomainEvents(Guid id, IDomainEvent domainEvent)
    {
        var attributeProxy = Attribute.CreateProxy(id);
        attributeRepository.Attach(attributeProxy);
        attributeProxy.AddDomainEvent(domainEvent);
    }
}