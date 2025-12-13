namespace CatalogService.Domain.Errors.EntitiesErrors;

public static partial class DomainErrors
{
    public class VariantAttributeDefinition
    {
        private const string _code = "variantAttributeDefinition";
        public static Error EmptyAllowedValues
            => Error.BadRequest(
                $"{_code}.{nameof(EmptyAllowedValues)}",
                "Allowed values cannot be empty");

        public static Error RequiredAllowedValues
            => Error.BadRequest(
                $"{_code}.{nameof(RequiredAllowedValues)}",
                "Allowed values are required for Select type variant attributes");

        public static Error NotApplicableAllowedValues
            => Error.BadRequest(
                $"{_code}.{nameof(NotApplicableAllowedValues)}",
                "Allowed values are not applicable for non-Select type variant attributes");
    }
}
