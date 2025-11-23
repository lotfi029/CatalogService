using Application.IntegrationTests.Infrastructure;
using CatalogService.Application.Features.Categories.Commands;

namespace Application.IntegrationTests;

public class CategoryTests(IntegrationTestWebAppFactory factory) 
    : BaseIntegrationTestsCommand<CreateCategoryCommand, Guid>(factory)
{
    [Fact]
    public async Task Create_Should_Add_NewProductToDatabase()
    {
        var command = new CreateCategoryCommand(
            Name: "Category Integration",
            Slug: "Integration",
            IsActive: true,
            ParentId: null,
            Description: "Description for category integration"
            );

        var result = await commandHandler.HandleAsync(command);

        Assert.True(result.IsSuccess);

    }
}
