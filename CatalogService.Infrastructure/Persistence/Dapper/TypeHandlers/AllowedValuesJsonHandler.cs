using CatalogService.Application.DTOs.CategoryVariantAttributes;
using CatalogService.Domain.JsonProperties;
using Dapper;
using System.Data;
using System.Text.Json;

namespace CatalogService.Infrastructure.Persistence.Dapper.TypeHandlers;

public sealed class AllowedValuesJsonHandler : SqlMapper.TypeHandler<AllowedValuesJson>
{
    public override AllowedValuesJson? Parse(object value)
    {
        if (value is null or DBNull)
            return null;

        var json = value.ToString();
        
        return string.IsNullOrWhiteSpace(json)
            ? null
            : JsonSerializer.Deserialize<AllowedValuesJson>(json);
    }

    public override void SetValue(IDbDataParameter parameter, AllowedValuesJson? value)
    {
        parameter.Value = value is not null
            ? JsonSerializer.Serialize(value)
            : DBNull.Value;
    }
}
public sealed class CategoryVariantForCategoryHandler : SqlMapper.TypeHandler<CategoryVariantForCategoryResponse>
{
    public override CategoryVariantForCategoryResponse? Parse(object value)
    {
        if (value is null or DBNull)
            return null;

        var response = (CategoryVariantForCategoryResponse)value;

        return response is null
            ? null
            : response;
    }

    public override void SetValue(IDbDataParameter parameter, CategoryVariantForCategoryResponse? value)
    {
        throw new NotImplementedException();
    }
}