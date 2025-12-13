namespace CatalogService.Domain.Errors.EntitiesErrors;

public static partial class DomainErrors
{
    public class Products
    {
        private const string _code = "products";
        public static Error UnspecifiedStatus
            => Error.BadRequest(
                $"{_code}.{nameof(UnspecifiedStatus)}",
                "Specify product status.");

        public static Error InvalidSkuSize(int defaultSize)
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidSkuSize)}",
                $"Sku length must be exact {defaultSize}");

        public static Error NotFound
            => Error.NotFound(
                $"{_code}.{nameof(NotFound)}",
                "the specific product cannot be found");
        public static Error InvalidStatusTransaction(string current, string newStatus)
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidStatusTransaction)}",
                $"Invalid status transaction {current} → {newStatus}");

        public static Error ProductAlreadyInStatus(string status)
            => Error.BadRequest(
                $"{_code}.{nameof(ProductAlreadyInStatus)}",
                $"Product Already in this status = {status}");
    }
}