using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Queries.GetById;

namespace CatalogService.IntegrationTests.Features.Categories.Queries;

public class GetCategoryByIdQueryHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTestsQuery<GetCategoryByIdQuery, CategoryDetailedResponse>(factory)
{
    [Fact]
    public async Task HandleAsync_WithExistingCategory_Should_ReturnCategory()
    {
        var category = Category.Create(
            "Electronics",
            "electronics",
            0,
            true,
            description: "Electronic products");

        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryByIdQuery(category.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().NotBeNull();
        result.Value!.Id.Should().Be(category.Id);
        result.Value!.Name.Should().Be("Electronics");
        result.Value!.Slug.Should().Be("electronics");
        result.Value!.Description.Should().Be("Electronic products");
        result.Value!.Path.Should().Be("electronics");
        result.Value!.Level.Should().Be(0);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentId_Should_ReturnNotFound()
    {
        var query = new GetCategoryByIdQuery(Guid.NewGuid());

        var result = await QueryHandler.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("NotFound");
    }

    [Fact]
    public async Task HandleAsync_WithDeletedCategory_Should_ReturnNotFound()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        category.Deleted();
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryByIdQuery(category.Id);

        var result = await QueryHandler.HandleAsync(query);
        var error = CategoryErrors.NotFound(category.Id);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain(error.Code);
    }

    [Fact]
    public async Task HandleAsync_WithCategoryWithParent_Should_ReturnParentId()
    {
        var parent = Category.Create("Electronics", "electronics", 0, true);
        var child = Category.Create("Computers", "computers", 1, true, parent.Id);

        AppDbContext.Categories.AddRange(parent, child);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryByIdQuery(child.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().Be(parent.Id);
        result.Value!.Level.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_WithRootCategory_Should_ReturnNullParentId()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryByIdQuery(category.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_WithEmptyGuid_Should_ReturnInvalidId()
    {
        var query = new GetCategoryByIdQuery(Guid.Empty);

        var result = await QueryHandler.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("Invalid");
    }

    [Fact]
    public async Task HandleAsync_MultipleTimes_Should_ReturnConsistentResults()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        var affected = await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryByIdQuery(category.Id);

        var result1 = await QueryHandler.HandleAsync(query);
        var result2 = await QueryHandler.HandleAsync(query);
        var result3 = await QueryHandler.HandleAsync(query);

        result1.Value.Should().BeEquivalentTo(result2.Value);
        result2.Value.Should().BeEquivalentTo(result3.Value);
    }

    [Fact]
    public async Task HandleAsync_WithComplexHierarchy_Should_ReturnCorrectLevel()
    {
        var root = Category.Create("Root", "root", 0, true);
        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryByIdQuery(level2.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Level.Should().Be(2);
        result.Value!.ParentId.Should().Be(level1.Id);
    }
}
