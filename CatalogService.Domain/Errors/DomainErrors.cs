using System.Reflection;
using System.Xml.Serialization;

namespace CatalogService.Domain.Errors;

public static class DomainErrors
{
    public class Attributes
    {
        private const string _code = "attributes";

        public static Error InvalidOptions
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidOptions)}",
                $"Options are only allowed for Select type");

        public static Error NullOptions
            => Error.BadRequest(
                $"{_code}.{nameof(NullOptions)}",
                "Options must be provided with select type");

        public static Error EmptyOptions
            => Error.BadRequest(
                $"{_code}.{EmptyOptions}",
                "Options cannot be empty");

        public static Error DuplicateOptions
            => Error.BadRequest(
                $"{_code}.{DuplicateOptions}",
                "Options cannot contain duplicates");

        public static Error OutOfRangeOptions(int maxValue)
            => Error.BadRequest(
                $"{_code}.{nameof(OutOfRangeOptions)}",
                $"Options cannot exceed {maxValue} entires");

        public static Error InvalidCastingEnum
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidCastingEnum)}",
                "Must specify the type of the Options");

        public static Error InvalidOptionType
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidOptionType)}",
                "Must specify the type for options");

        public static Error InvalidActiveOperation
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidActiveOperation)}",
                "this attribute is active already");
        
        public static Error InvalidDeactiveOperation
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidDeactiveOperation)}",
                "this attribute is deactive already");

        public static Error NotFound
            => Error.BadRequest(
                $"{_code}.{nameof(NotFound)}",
                "this attribute is not found");
    }

    public static Error Null(string code) 
        => Error.BadRequest(code, "value cannot be empty or white space");
    public static Error NullNumber(string code) 
        => Error.BadRequest(code, "value cannot be less than or equal 0");
    
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
    public class ProductVariants
    {
        private const string _code = "productVariants";

        //public static Error 
    }
}