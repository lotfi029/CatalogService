using CatalogService.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CatalogService.Domain.Errors;

public sealed class CategoryErrors
{
    private const string _code = "code";
    public static Error ParentNotFound(Guid parentId)
        => Error.NotFound(
            $"{_code}.{nameof(ParentNotFound)}",
            $"Parent category with ID {parentId} dos not exist");

    public static Error SlugAlreadyExist(string slug)
        => Error.NotFound(
            $"{_code}.{nameof(SlugAlreadyExist)}",
            $"Category with slug: '{slug}' is already exist");
}
