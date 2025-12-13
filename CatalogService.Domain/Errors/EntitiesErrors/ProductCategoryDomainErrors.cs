namespace CatalogService.Domain.Errors.EntitiesErrors;

public static partial class DomainErrors
{
    public class ProductCategories
    {
        private const string _code = "productCategories";

        public static Error AlreadyPrimary
            => Error.BadRequest(
                $"{_code}.{nameof(AlreadyPrimary)}",
                "The product category is already marked as primary");

        public static Error AlreadyNotPrimary
            => Error.BadRequest(
                $"{_code}.{nameof(AlreadyNotPrimary)}",
                "The product category is already not primary");

        public static Error AlreadyDeleted
            => Error.BadRequest(
                $"{_code}.{nameof(AlreadyDeleted)}",
                "The product category is already removed");

        public static Error AlreadyNotDeleted
            => Error.BadRequest(
                $"{_code}.{nameof(AlreadyNotDeleted)}",
                "The product category is already in list of products");
    }
}
