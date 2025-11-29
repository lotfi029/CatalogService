using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Queries.Tree;


namespace CatalogService.IntegrationTests.Features.Categories.Queries;

public class GetCategoryTreeQueryHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTestsQuery<GetCategoryTreeQuery, IEnumerable<CategoryResponse>>(factory)
{
    [Fact]
    public async Task HandleAsync_WithNoCategoriesExist_Should_ReturnEmptyCollection()
    {
        var query = new GetCategoryTreeQuery(null);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithNullParentId_Should_ReturnAllCategories()
    {
        var category1 = Category.Create("Electronics", "electronics", 0, true);
        var category2 = Category.Create("Books", "books", 0, true);

        AppDbContext.Categories.AddRange(category1, category2);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(null);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCountGreaterThanOrEqualTo(2);
        result.Value.Should().Contain(c => c.Slug == "electronics");
        result.Value.Should().Contain(c => c.Slug == "books");
    }

    [Fact]
    public async Task HandleAsync_WithEmptyParentId_Should_ReturnAllCategories()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(Guid.Empty);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithValidParentId_Should_ReturnCategoryTree()
    {
        var parent = Category.Create("Electronics", "electronics", 0, true);
        var child1 = Category.Create("Laptops", "laptops", 1, true, parent.Id, parentPath: parent.Path);
        var child2 = Category.Create("Phones", "phones", 1, true, parent.Id, parentPath: parent.Path);

        AppDbContext.Categories.AddRange(parent, child1, child2);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(parent.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Contain(c => c.Id == parent.Id);
        result.Value.Should().Contain(c => c.ParentId == parent.Id);
    }

    [Fact]
    public async Task HandleAsync_WithDeletedCategories_Should_ExcludeDeleted()
    {
        var activeCategory = Category.Create("Active", "active", 0, true);
        var deletedCategory = Category.Create("Deleted", "deleted", 0, true);

        AppDbContext.Categories.AddRange(activeCategory, deletedCategory);
        await AppDbContext.SaveChangesAsync();

        deletedCategory.Delete();
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(null);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Contain(c => c.Name == "Active");
        result.Value.Should().NotContain(c => c.Name == "Deleted");
    }

    [Fact]
    public async Task HandleAsync_WithMultipleLevels_Should_ReturnHierarchy()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var level1 = Category.Create("Laptops", "laptops", 1, true, root.Id, parentPath: root.Path);
        var level2 = Category.Create("Gaming", "gaming", 2, true, level1.Id, parentPath: level1.Path);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(root.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Contain(c => c.Level == 0);
        result.Value.Should().Contain(c => c.Level == 1);
        result.Value.Should().Contain(c => c.Level == 2);
    }

    [Fact]
    public async Task HandleAsync_WithDeepHierarchy_Should_ReturnAllLevels()
    {
        var level0 = Category.Create("Root", "root", 0, true);
        var level1 = Category.Create("Level1", "level1", 1, true, level0.Id, parentPath: level0.Path);
        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id, parentPath: level1.Path);
        var level3 = Category.Create("Level3", "level3", 3, true, level2.Id, parentPath: level2.Path);

        AppDbContext.Categories.AddRange(level0, level1, level2, level3);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(level0.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(4);
        result.Value.Select(c => c.Level).Should().Contain([0, 1, 2, 3]);
    }

    [Fact]
    public async Task HandleAsync_WithMultipleBranches_Should_ReturnAllBranches()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var branch1 = Category.Create("Laptops", "laptops", 1, true, root.Id, parentPath: root.Path);
        var branch2 = Category.Create("Phones", "phones", 1, true, root.Id, parentPath: root.Path);
        var leaf1 = Category.Create("Gaming", "gaming", 2, true, branch1.Id, parentPath: branch1.Path);
        var leaf2 = Category.Create("Smartphones", "smartphones", 2, true, branch2.Id, parentPath: branch2.Path);

        AppDbContext.Categories.AddRange(root, branch1, branch2, leaf1, leaf2);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(root.Id);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(5);
    }

    [Fact]
    public async Task HandleAsync_MultipleTimes_Should_ReturnConsistentResults()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(null);

        var result1 = await QueryHandler.HandleAsync(query);
        var result2 = await QueryHandler.HandleAsync(query);
        var result3 = await QueryHandler.HandleAsync(query);

        result1.Value.Should().BeEquivalentTo(result2.Value);
        result2.Value.Should().BeEquivalentTo(result3.Value);
    }

    [Fact]
    public async Task HandleAsync_WithInactiveCategories_Should_ReturnInactiveCategories()
    {
        var activeCategory = Category.Create("Active", "active", 0, true);
        var inactiveCategory = Category.Create("Inactive", "inactive", 0, false);

        AppDbContext.Categories.AddRange(activeCategory, inactiveCategory);
        await AppDbContext.SaveChangesAsync();

        var query = new GetCategoryTreeQuery(null);

        var result = await QueryHandler.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Contain(c => c.Name == "Active");
        result.Value.Should().Contain(c => c.Name == "Inactive");
    }
}