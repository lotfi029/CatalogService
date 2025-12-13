namespace CatalogService.Domain.Errors.EntitiesErrors;

public static partial class DomainErrors
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
}