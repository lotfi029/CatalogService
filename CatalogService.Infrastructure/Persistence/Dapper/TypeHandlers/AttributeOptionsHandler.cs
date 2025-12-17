using System.Data;
using System.Text.Json;

namespace CatalogService.Infrastructure.Persistence.Dapper.TypeHandlers;

public sealed class AttributeOptionsHandler : SqlMapper.TypeHandler<HashSet<string>?>
{
    public override HashSet<string>? Parse(object value)
    {
        if (value is null or DBNull)
            return null;

        var json = value.ToString();

        return string.IsNullOrWhiteSpace(json) 
            ? null
            : JsonSerializer.Deserialize<HashSet<string>>(json);
    }

    public override void SetValue(IDbDataParameter parameter, HashSet<string>? value)
    {
        parameter.Value = value is not null
            ? JsonSerializer.Serialize(value)
            : DBNull.Value;
    }
}