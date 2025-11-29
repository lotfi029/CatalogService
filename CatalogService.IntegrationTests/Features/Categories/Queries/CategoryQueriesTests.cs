using CatalogService.Application.Features.Categories.Queries;

namespace CatalogService.IntegrationTests.Features.Categories.Queries;

public class CategoryQueriesTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTests(factory), IAsyncLifetime 
{
    private ICategoryQueries _queries { get; set; } = null!;
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _queries = GetService<ICategoryQueries>();
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingCategory_Should_ReturnCategory()
    {
        var category = Category.Create(
            "Electronics",
            "electronics",
            0,
            true,
            description: "Electronic products");

        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetByIdAsync(category.Id);

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
    public async Task GetByIdAsync_WithNonExistentId_Should_ReturnNotFound()
    {
        var result = await _queries.GetByIdAsync(Guid.NewGuid());

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("NotFound");
    }

    [Fact]
    public async Task GetByIdAsync_WithDeletedCategory_Should_ReturnNotFound()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        category.Delete();
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetByIdAsync(category.Id);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("NotFound");
    }

    [Fact]
    public async Task GetByIdAsync_WithCategoryWithParent_Should_ReturnParentId()
    {
        var parent = Category.Create("Electronics", "electronics", 0, true);
        var child = Category.Create("Laptops", "laptops", 1, true, parent.Id, parentPath: parent.Path);

        AppDbContext.Categories.AddRange(parent, child);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetByIdAsync(child.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().Be(parent.Id);
        result.Value!.Level.Should().Be(1);
        result.Value!.Path.Should().Be("electronics/laptops");
    }

    [Fact]
    public async Task GetByIdAsync_WithRootCategory_Should_ReturnNullParentId()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetByIdAsync(category.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithNullDescription_Should_ReturnNullDescription()
    {
        var category = Category.Create("Electronics", "electronics", 0, true, description: null);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetByIdAsync(category.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Description.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithComplexHierarchy_Should_ReturnCorrectLevel()
    {
        var root = Category.Create("Root", "root", 0, true);
        var level1 = Category.Create("Level1", "level1", 1, true, root.Id, parentPath: root.Path);
        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id, parentPath: level1.Path);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetByIdAsync(level2.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Level.Should().Be(2);
        result.Value!.ParentId.Should().Be(level1.Id);
        result.Value!.Path.Should().Be("root/level1/level2");
    }

    [Fact]
    public async Task GetByIdAsync_MultipleTimes_Should_ReturnConsistentResults()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result1 = await _queries.GetByIdAsync(category.Id);
        var result2 = await _queries.GetByIdAsync(category.Id);
        var result3 = await _queries.GetByIdAsync(category.Id);

        result1.Value.Should().BeEquivalentTo(result2.Value);
        result2.Value.Should().BeEquivalentTo(result3.Value);
    }

    [Fact]
    public async Task GetBySlugAsync_WithExistingCategory_Should_ReturnCategory()
    {
        var category = Category.Create(
            "Electronics",
            "electronics",
            0,
            true,
            description: "Electronic products");

        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetBySlugAsync("electronics");

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().NotBeNull();
        result.Value!.Slug.Should().Be("electronics");
        result.Value!.Name.Should().Be("Electronics");
        result.Value!.Description.Should().Be("Electronic products");
        result.Value!.Level.Should().Be(0);
    }

    [Fact]
    public async Task GetBySlugAsync_WithNonExistentSlug_Should_ReturnSlugNotFound()
    {
        var result = await _queries.GetBySlugAsync("non-existent");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("SlugNotFound");
    }

    [Fact]
    public async Task GetBySlugAsync_WithDeletedCategory_Should_ReturnSlugNotFound()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        category.Delete();
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetBySlugAsync("electronics");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("SlugNotFound");
    }

    [Fact]
    public async Task GetBySlugAsync_WithCategoryWithParent_Should_ReturnParentId()
    {
        var parent = Category.Create("Electronics", "electronics", 0, true);
        var child = Category.Create("Laptops", "laptops", 1, true, parent.Id, parentPath: parent.Path);

        AppDbContext.Categories.AddRange(parent, child);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetBySlugAsync("laptops");

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().Be(parent.Id);
        result.Value!.Level.Should().Be(1);
    }

    [Fact]
    public async Task GetBySlugAsync_WithRootCategory_Should_ReturnNullParentId()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetBySlugAsync("electronics");

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().BeNull();
    }

    [Fact]
    public async Task GetBySlugAsync_WithComplexPath_Should_ReturnFullPath()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var level1 = Category.Create("Laptops", "laptops", 1, true, root.Id, parentPath: root.Path);
        var level2 = Category.Create("Gaming", "gaming", 2, true, level1.Id, parentPath: level1.Path);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetBySlugAsync("gaming");

        result.IsSuccess.Should().BeTrue();
        result.Value!.Path.Should().Be("electronics/laptops/gaming");
        result.Value!.Level.Should().Be(2);
    }

    [Theory]
    [InlineData("electronics")]
    [InlineData("books-fiction")]
    [InlineData("home-garden")]
    public async Task GetBySlugAsync_WithDifferentSlugs_Should_ReturnCorrectCategory(string slug)
    {
        var category = Category.Create("Category", slug, 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetBySlugAsync(slug);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Slug.Should().Be(slug);
    }

    [Fact]
    public async Task GetBySlugAsync_MultipleTimes_Should_ReturnConsistentResults()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result1 = await _queries.GetBySlugAsync("electronics");
        var result2 = await _queries.GetBySlugAsync("electronics");
        var result3 = await _queries.GetBySlugAsync("electronics");

        result1.Value.Should().BeEquivalentTo(result2.Value);
        result2.Value.Should().BeEquivalentTo(result3.Value);
    }

    [Fact]
    public async Task GetTreeAsync_WithNullParentId_Should_ReturnAllCategories()
    {
        var category1 = Category.Create("Electronics", "electronics", 0, true);
        var category2 = Category.Create("Books", "books", 0, true);

        AppDbContext.Categories.AddRange(category1, category2);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(null);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCountGreaterThanOrEqualTo(2);
        result.Value!.Should().Contain(c => c.Slug == "electronics");
        result.Value!.Should().Contain(c => c.Slug == "books");
    }

    [Fact]
    public async Task GetTreeAsync_WithEmptyParentId_Should_ReturnAllCategories()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(Guid.Empty);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetTreeAsync_WithValidParentId_Should_ReturnCategoryTree()
    {
        var parent = Category.Create("Electronics", "electronics", 0, true);
        var child1 = Category.Create("Laptops", "laptops", 1, true, parent.Id, parentPath: parent.Path);
        var child2 = Category.Create("Phones", "phones", 1, true, parent.Id, parentPath: parent.Path);

        AppDbContext.Categories.AddRange(parent, child1, child2);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(parent.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().NotBeEmpty();
        result.Value!.Should().Contain(c => c.Id == parent.Id);
        result.Value!.Should().Contain(c => c.ParentId == parent.Id);
    }

    [Fact]
    public async Task GetTreeAsync_WithDeletedCategories_Should_ExcludeDeleted()
    {
        var activeCategory = Category.Create("Active", "active", 0, true);
        var deletedCategory = Category.Create("Deleted", "deleted", 0, true);

        AppDbContext.Categories.AddRange(activeCategory, deletedCategory);
        await AppDbContext.SaveChangesAsync();

        deletedCategory.Delete();
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(null);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().Contain(c => c.Name == "Active");
        result.Value!.Should().NotContain(c => c.Name == "Deleted");
    }

    [Fact]
    public async Task GetTreeAsync_WithMultipleLevels_Should_ReturnHierarchy()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var level1 = Category.Create("Laptops", "laptops", 1, true, root.Id, parentPath: root.Path);
        var level2 = Category.Create("Gaming", "gaming", 2, true, level1.Id, parentPath: level1.Path);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(root.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().Contain(c => c.Level == 0);
        result.Value!.Should().Contain(c => c.Level == 1);
        result.Value!.Should().Contain(c => c.Level == 2);
    }

    [Fact]
    public async Task GetTreeAsync_WithDeepHierarchy_Should_ReturnAllLevels()
    {
        var level0 = Category.Create("Root", "root", 0, true);
        var level1 = Category.Create("Level1", "level1", 1, true, level0.Id, parentPath: level0.Path);
        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id, parentPath: level1.Path);
        var level3 = Category.Create("Level3", "level3", 3, true, level2.Id, parentPath: level2.Path);

        AppDbContext.Categories.AddRange(level0, level1, level2, level3);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(level0.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(4);
        result.Value!.Select(c => c.Level).Should().Contain(new short[] { 0, 1, 2, 3 });
    }

    [Fact]
    public async Task GetTreeAsync_WithMultipleBranches_Should_ReturnAllBranches()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var branch1 = Category.Create("Laptops", "laptops", 1, true, root.Id, parentPath: root.Path);
        var branch2 = Category.Create("Phones", "phones", 1, true, root.Id, parentPath: root.Path);
        var leaf1 = Category.Create("Gaming", "gaming", 2, true, branch1.Id, parentPath: branch1.Path);
        var leaf2 = Category.Create("Smartphones", "smartphones", 2, true, branch2.Id, parentPath: branch2.Path);

        AppDbContext.Categories.AddRange(root, branch1, branch2, leaf1, leaf2);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(root.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetTreeAsync_WithNoCategoriesExist_Should_ReturnEmptyCollection()
    {
        var result = await _queries.GetTreeAsync(null);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTreeAsync_WithNonExistentParentId_Should_ReturnEmptyCollection()
    {
        var result = await _queries.GetTreeAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTreeAsync_MultipleTimes_Should_ReturnConsistentResults()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var result1 = await _queries.GetTreeAsync(null);
        var result2 = await _queries.GetTreeAsync(null);
        var result3 = await _queries.GetTreeAsync(null);

        result1.Value.Should().BeEquivalentTo(result2.Value);
        result2.Value.Should().BeEquivalentTo(result3.Value);
    }

    [Fact]
    public async Task GetTreeAsync_WithInactiveCategories_Should_ReturnInactiveCategories()
    {
        var activeCategory = Category.Create("Active", "active", 0, true);
        var inactiveCategory = Category.Create("Inactive", "inactive", 0, false);

        AppDbContext.Categories.AddRange(activeCategory, inactiveCategory);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(null);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().Contain(c => c.Name == "Active");
        result.Value!.Should().Contain(c => c.Name == "Inactive");
    }

    [Fact]
    public async Task GetTreeAsync_WithDeletedParent_Should_ExcludeEntireSubtree()
    {
        var parent = Category.Create("Electronics", "electronics", 0, true);
        var child = Category.Create("Laptops", "laptops", 1, true, parent.Id, parentPath: parent.Path);

        AppDbContext.Categories.AddRange(parent, child);
        await AppDbContext.SaveChangesAsync();

        parent.Delete();
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(parent.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTreeAsync_WithOnlyRootCategories_Should_ReturnRootCategories()
    {
        var root1 = Category.Create("Electronics", "electronics", 0, true);
        var root2 = Category.Create("Books", "books", 0, true);
        var root3 = Category.Create("Clothing", "clothing", 0, true);

        AppDbContext.Categories.AddRange(root1, root2, root3);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(null);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(3);
        result.Value!.Should().OnlyContain(c => c.Level == 0);
    }

    [Fact]
    public async Task GetTreeAsync_WithSpecificBranch_Should_ReturnOnlyThatBranch()
    {
        var root = Category.Create("Root", "root", 0, true);
        var branch1 = Category.Create("Branch1", "branch1", 1, true, root.Id, parentPath: root.Path);
        var branch2 = Category.Create("Branch2", "branch2", 1, true, root.Id, parentPath: root.Path);
        var leaf1 = Category.Create("Leaf1", "leaf1", 2, true, branch1.Id, parentPath: branch1.Path);

        AppDbContext.Categories.AddRange(root, branch1, branch2, leaf1);
        await AppDbContext.SaveChangesAsync();

        var result = await _queries.GetTreeAsync(branch1.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().Contain(c => c.Id == branch1.Id);
        result.Value!.Should().Contain(c => c.Id == leaf1.Id);
        result.Value!.Should().NotContain(c => c.Id == branch2.Id);
    }
}
