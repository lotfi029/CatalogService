using CatalogService.Domain.Enums;
using Dapper;
using System.Data;

namespace CatalogService.Infrastructure.Persistence.Dapper.TypeHandlers;

public sealed class VariantDatatypeHandler : SqlMapper.TypeHandler<VariantsType>
{
    public override VariantsType? Parse(object value)
    {
        if (value is null or DBNull)
            return null!;

        var enumValue = value is short shortValues
            ? (VariantDataType)shortValues
            : (VariantDataType)Convert.ToInt16(value);

        return new VariantsType(enumValue);
    }

    public override void SetValue(IDbDataParameter parameter, VariantsType? value)
    {
        parameter.Value = value?.DataType ?? (object)DBNull.Value;
    }
}