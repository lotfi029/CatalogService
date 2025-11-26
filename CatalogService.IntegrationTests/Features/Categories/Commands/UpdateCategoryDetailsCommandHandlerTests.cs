using CatalogService.Application.Features.Categories.Commands.UpdateDetails;
using CatalogService.IntegrationTests.Infrastructure;

namespace CatalogService.IntegrationTests.Features.Categories.Commands;

public class UpdateCategoryDetailsCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTestsCommand<UpdateCategoryDetailsCommand>(factory)
{
}
