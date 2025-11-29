using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Queries.GetBySlug;
using CatalogService.Domain.Entities;
using CatalogService.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace CatalogService.IntegrationTests.Features.Categories.Queries;

public class GetCategoryBySlugQueryHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTestsQuery<GetCategoryBySlugQuery, CategoryDetailedResponse>(factory)
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

        var query = new GetCategoryBySlugQuery("electronics");

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().NotBeNull();
        result.Value!.Slug.Should().Be("electronics");
        result.Value!.Name.Should().Be("Electronics");
        result.Value!.Description.Should().Be("Electronic products");
        result.Value!.Level.Should().Be(0);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentSlug_Should_ReturnSlugNotFound()
    {
        var query = new GetCategoryBySlugQuery("non-existent");

        var result = await QueryHandler.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("SlugNotFound");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task HandleAsync_WithInvalidSlug_Should_ReturnInvalidSlug(string invalidSlug)
    {
        var query = new GetCategoryBySlugQuery(invalidSlug);

        var result = await QueryHandler.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("Invalid");
    }

    [Fact]
    public async Task HandleAsync_WithDeletedCategory_Should_ReturnSlugNotFound()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        category.Delete();
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryBySlugQuery("electronics");

        var result = await QueryHandler.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task HandleAsync_WithCategoryWithParent_Should_ReturnParentId()
    {
        var parent = Category.Create("Electronics", "electronics", 0, true);
        var child = Category.Create("Laptops", "laptops", 1, true, parent.Id, parentPath: parent.Path);

        AppDbContext.Categories.AddRange(parent, child);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryBySlugQuery("laptops");

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

        var query = new GetCategoryBySlugQuery("electronics");

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_WithNullDescription_Should_ReturnNullDescription()
    {
        var category = Category.Create("Electronics", "electronics", 0, true, description: null);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryBySlugQuery("electronics");

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Description.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_MultipleTimes_Should_ReturnConsistentResults()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryBySlugQuery("electronics");

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
        var level1 = Category.Create("Level1", "level1", 1, true, root.Id, parentPath: root.Path);
        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id, parentPath: level1.Path);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryBySlugQuery("level2");

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Level.Should().Be(2);
        result.Value!.ParentId.Should().Be(level1.Id);
    }

    [Fact]
    public async Task HandleAsync_WithComplexPath_Should_ReturnFullPath()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var level1 = Category.Create("Laptops", "laptops", 1, true, root.Id, parentPath: root.Path);
        var level2 = Category.Create("Gaming", "gaming", 2, true, level1.Id, parentPath: level1.Path);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryBySlugQuery("gaming");

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Path.Should().Be("electronics/laptops/gaming");
        result.Value!.Level.Should().Be(2);
    }
}