using CatalogService.Domain.DomainService.Attributes;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.Features.Attributes.Command.Create;

public sealed record CreateAttributeCommand(string Name, string Code, string OptionType, bool IsFilterable, bool IsSearchable, ValuesJson? Options) : ICommand<Guid>;

internal sealed class CreateAttributeCommandHandler(
    ILogger<CreateAttributeCommandHandler> logger,
    IAttributeDomainService attributeService,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateAttributeCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateAttributeCommand command, CancellationToken ct = default)
    {
        try
        {
            var result = await attributeService.CreateAsync(
                name: command.Name,
                code: command.Code,
                optionType: command.OptionType,
                isFilterable: command.IsFilterable,
                isSearchable: command.IsSearchable,
                valuesJson: command.Options,
                ct: ct);

            if (result.IsFailure)
                return result;

            await unitOfWork.SaveChangesAsync(ct);
            
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while creating new attribute");
            return AttributeErrors.CreateAttribute;
        }
    }
}