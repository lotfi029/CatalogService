namespace CatalogService.Domain.Errors.EntitiesErrors;

public static partial class DomainErrors
{
    public class CategoryVariantAttributes
    {
        private const string _code = "categoryVariantAttributes";

        public static Error AlreadyRequired
            => Error.BadRequest(
                $"{_code}.{nameof(AlreadyRequired)}",
                "The variant attribute is already marked as required for this category");

        public static Error AlreadyNotRequired
            => Error.BadRequest(
                $"{_code}.{nameof(AlreadyNotRequired)}",
                "The variant attribute is already marked as not required for this category");

        public static Error AlraedyNotDeleted
            => Error.BadRequest(
                $"{_code}.{nameof(AlraedyNotDeleted)}",
                "The variant attribute is already in list of categories");

        public static Error AlreadyDeleted
            => Error.BadRequest(
                $"{_code}.{nameof(AlreadyDeleted)}",
                "The variant attribute is already removed from this category");
    }
}
