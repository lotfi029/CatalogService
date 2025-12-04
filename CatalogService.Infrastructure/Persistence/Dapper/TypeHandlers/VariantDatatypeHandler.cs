using CatalogService.Domain.Enums;
using Dapper;
using System.Data;

namespace CatalogService.Infrastructure.Persistence.Dapper.TypeHandlers;

public sealed class VariantDatatypeHandler : SqlMapper.TypeHandler<OptionsType>
{
    public override OptionsType? Parse(object value)
    {
        if (value is null or DBNull)
            return null!;

        var enumValue = value is short shortValues
            ? (ValuesDataType)shortValues
            : (ValuesDataType)Convert.ToInt16(value);

        return new OptionsType(enumValue);
    }

    public override void SetValue(IDbDataParameter parameter, OptionsType? value)
    {
        parameter.Value = value?.DataType ?? (object)DBNull.Value;
    }
}