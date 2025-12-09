using CatalogService.Infrastructure.Persistence.Dapper.TypeHandlers;
using Dapper;

namespace CatalogService.Infrastructure.Persistence.Dapper;

public static class DapperConfiguration
{
    private static bool _isConfigured;

    public static void Configure()
    {
        if (_isConfigured) return;

        //SqlMapper.AddTypeHandler(new VariantDatatypeHandler());
        SqlMapper.AddTypeHandler(new AllowedValuesJsonHandler());
        SqlMapper.AddTypeHandler(new ProductVariantsOptionHandler());

        _isConfigured = true;
    }
}
