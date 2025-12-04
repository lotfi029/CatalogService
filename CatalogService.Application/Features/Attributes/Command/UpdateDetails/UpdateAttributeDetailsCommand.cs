namespace CatalogService.Application.Features.Attributes.Command.UpdateDetails;

public sealed record UpdateAttributeDetailsCommand(Guid Id, string Name, bool IsFilterable, bool IsSearchable) : ICommand;
