using CatalogService.Domain.Enums;
using Dapper;
using System.Data;

namespace CatalogService.Infrastructure.Persistence.Dapper.TypeHandlers;

public sealed class VariantDatatypeHandler : SqlMapper.TypeHandler<VariantDatatype>
{
    public override VariantDatatype? Parse(object value)
    {
        if (value is null or DBNull)
            return null!;

        var enumValue = value is short shortValues
            ? (VaraintAttributeDatatype)shortValues
            : (VaraintAttributeDatatype)Convert.ToInt16(value);

        return new VariantDatatype(enumValue);
    }

    public override void SetValue(IDbDataParameter parameter, VariantDatatype? value)
    {
        parameter.Value = value?.Datatype ?? (object)DBNull.Value;
    }
}