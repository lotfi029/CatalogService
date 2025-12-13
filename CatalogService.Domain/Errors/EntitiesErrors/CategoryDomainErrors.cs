namespace CatalogService.Domain.Errors.EntitiesErrors;

public static partial class DomainErrors
{
    public class Categories
    {
        private const string _code = "categories";

        public static Error InvalidName
            => Error.BadRequest(
                $"{_code}.{nameof(InvalidName)}",
                "Please enter a valid name");

        public static Error AlreadyDeleted
            => Error.Conflict(
                $"{_code}.{nameof(AlreadyDeleted)}",
                "This category is already deleted");

        public static Error SlugIsRequired
            => Error.BadRequest(
                $"{_code}.{nameof(SlugIsRequired)}",
                "Slug is required");

        public static Error NameIsRequired
            => Error.BadRequest(
                $"{_code}.{nameof(NameIsRequired)}",
                "Name is required");

        public static Error LevelIsRequired
            => Error.BadRequest(
                $"{_code}.{nameof(LevelIsRequired)}",
                "Level cannot be negative");
    }
}