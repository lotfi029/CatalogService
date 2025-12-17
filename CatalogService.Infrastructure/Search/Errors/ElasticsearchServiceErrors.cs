namespace CatalogService.Infrastructure.Search.Errors;

internal sealed class ElasticsearchServiceErrors
{
    public static Error IndexedFailed
        => Error.BadRequest("Indexed Failed", "indexed failed");
    
    public static Error UpdatedFailed
        => Error.BadRequest("UpdatedFailed", "Updated failed");
    public static Error DeletedFailed
        => Error.BadRequest("DeletedFailed", "Deleted failed");
    public static Error NotFound
        => Error.BadRequest("NotFound", "NotFound document");
}
