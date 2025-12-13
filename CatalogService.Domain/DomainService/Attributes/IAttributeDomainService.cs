using CatalogService.Domain.JsonProperties;

namespace CatalogService.Domain.DomainService.Attributes;

public interface IAttributeDomainService
{
    Task<Result<Guid>> CreateAsync(
        string name,
        string code,
        string optionType,
        bool isFilterable,
        bool isSearchable,
        ValuesJson? valuesJson,
        CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<Result> UpdateDetailsAsync(Guid id, string name, bool isFilterable, bool isSearchable, CancellationToken ct = default);
    Task<Result> UpdateOptionsAsync(Guid id, ValuesJson options, CancellationToken ct = default);
}
