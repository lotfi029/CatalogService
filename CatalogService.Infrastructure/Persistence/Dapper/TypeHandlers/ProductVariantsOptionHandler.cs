using CatalogService.Domain.JsonProperties;
using System.Data;
using System.Text.Json;

namespace CatalogService.Infrastructure.Persistence.Dapper.TypeHandlers;

public sealed class ProductVariantsOptionHandler : SqlMapper.TypeHandler<ProductVariantsOption>
{
    public override ProductVariantsOption? Parse(object value)
    {
        if (value is null or DBNull)
            return null;

        var json = value.ToString();

        return string.IsNullOrWhiteSpace(json)
            ? null
            : JsonSerializer.Deserialize<ProductVariantsOption>(json);
    }

    public override void SetValue(IDbDataParameter parameter, ProductVariantsOption? value)
    {
        parameter.Value = value is not null
            ? JsonSerializer.Serialize(value)
            : DBNull.Value;
    }
}