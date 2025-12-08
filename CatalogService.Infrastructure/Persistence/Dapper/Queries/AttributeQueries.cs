using CatalogService.Application.DTOs.Attributes;
using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Domain.Enums;
namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

public sealed class AttributeQueries(
    IDbConnectionFactory connectionFactory) : IAttributeQueries
{
    public async Task<Result<AttributeDetailedResponse>> GetAsync(Guid id, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT 
                a.id as Id,
                a.name as Name,
                a.code as Code,
                a.options_type_name as OptionsType,
                a.options as Options,
                a.is_active as IsActive,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable,
                a.created_at as CreatedAt,
                a.last_updated_at as UpdatedAt
            FROM public.attributes a
            WHERE a.id = @id
                AND a.is_active = true
                AND a.is_deleted = false
            """;
        var parameters = new { id };
        var result = await connection.QuerySingleOrDefaultAsync<AttributeDetailedResponse>(
            new CommandDefinition(
                commandText: sql, parameters: parameters, cancellationToken: ct));

        if (result is null)
            return AttributeErrors.NotFound(id);


        return result;
    }
    public async Task<Result<AttributeDetailedResponse>> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT 
                a.id as Id,
                a.name as Name,
                a.code as Code,
                a.options_type_name as OptionsType,
                a.options as Options,
                a.is_active as IsActive,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable,
                a.created_at as CreatedAt,
                a.last_updated_at as UpdatedAt
            FROM public.attributes a
            WHERE a.code = @code
                AND a.is_active = true
                AND a.is_deleted = false
            """;
        var parameters = new { code };
        var result = await connection.QuerySingleOrDefaultAsync<AttributeDetailedResponse>(
            new CommandDefinition(
                commandText: sql, parameters: parameters, cancellationToken: ct));

        if (result is null)
            return AttributeErrors.CodeNotFound(code);

        return result;
    }
    public async Task<Result<IEnumerable<AttributeResponse>>> GetByOptionsTypeAsync(string type, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        if (!Enum.TryParse<VariantDataType>(type, ignoreCase: true, out var enumOptionType))
            return AttributeErrors.InvalidOptinsType;

        var sql = """
            SELECT 
                a.id as Id,
                a.name as Name,
                a.code as Code,
                a.options_type_name as OptionsType,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable
                
            FROM public.attributes a
            WHERE a.options_type = @enumOptionType
                AND a.is_active = true
                AND a.is_deleted = false
            """;

        var parameters = new { enumOptionType };

        var command = new CommandDefinition(commandText: sql, parameters: parameters, cancellationToken: ct);
        var response = await connection.QueryAsync<AttributeResponse>(command);

        return Result.Success(response);
    }
    public async Task<Result<IEnumerable<AttributeResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT 
                a.id as Id,
                a.name as Name,
                a.code as Code,
                a.options_type_name as OptionsType,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable
            FROM public.attributes a
            WHERE a.is_active = true
                AND a.is_deleted = false
            """;

        var response = await connection.QueryAsync<AttributeResponse>(new CommandDefinition(commandText: sql, cancellationToken: ct));

        return Result.Success(response);
    }
}