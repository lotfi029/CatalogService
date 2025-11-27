using CatalogService.Domain.Entities;
using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence.Repositories;
using CatalogService.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
namespace CatalogService.IntegrationTests.Repositories;
public class CategoryRepositoryIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTests(factory)
{
    CategoryRepository repository = null!;

    public async override Task InitializeAsync()
    {
        await base.InitializeAsync();
        repository = new CategoryRepository(AppDbContext);
    }

    [Fact]
    public async Task Add_WithValidEntity_Should_PersistToDatabase()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        repository.Add(category);
        await AppDbContext.SaveChangesAsync();

        var saved = await AppDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == category.Id);

        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task Update_WithValidEntity_Should_PersistChangesToDatabase()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        category.UpdateDetails("Electronics Updated", "New description");
        repository.Update(category);
        await AppDbContext.SaveChangesAsync();

        var updated = await AppDbContext.Categories
            .AsNoTracking()
            .FirstAsync(c => c.Id == category.Id);

        updated.Name.Should().Be("Electronics Updated");
        updated.Description.Should().Be("New description");
    }

    [Fact]
    public async Task Remove_WithValidEntity_Should_DeleteFromDatabase()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        repository.Remove(category);
        await AppDbContext.SaveChangesAsync();

        var exists = await AppDbContext.Categories.AnyAsync(c => c.Id == category.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsAsync_ById_WithExistingEntity_Should_ReturnTrue()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var exists = await repository.ExistsAsync(category.Id);

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task FindByIdAsync_WithExistingEntity_Should_ReturnEntity()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var found = await repository.FindByIdAsync(category.Id);

        found.Should().NotBeNull();
        found!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task FindAsync_WithPredicate_Should_ReturnMatchingEntity()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(category);
        await AppDbContext.SaveChangesAsync();

        var found = await repository.FindAsync(c => c.Slug == "electronics");

        found.Should().NotBeNull();
        found!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task ExecuteDeleteAsync_WithPredicate_Should_DeleteMatchingEntities()
    {
        var cat1 = Category.Create("Cat1", "cat1", 0, false);
        var cat2 = Category.Create("Cat2", "cat2", 0, false);
        var cat3 = Category.Create("Cat3", "cat3", 0, true);

        AppDbContext.Categories.AddRange(cat1, cat2, cat3);
        await AppDbContext.SaveChangesAsync();

        var deletedCount = await repository.ExecuteDeleteAsync(c => !c.IsActive);

        deletedCount.Should().Be(2);

        var remaining = await AppDbContext.Categories.CountAsync();
        remaining.Should().Be(1);
    }
    [Fact]
    public async Task GetAllParentAsync_WithDeepHierarchy_Should_ReturnAllAncestors()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var level1 = Category.Create("Computers", "computers", 1, true, root.Id, path: "electronics");
        var level2 = Category.Create("Laptops", "laptops", 2, true, level1.Id, path: "electronics/computers");
        var level3 = Category.Create("Gaming", "gaming", 3, true, level2.Id, path: "electronics/computers/laptops");

        AppDbContext.Categories.AddRange(root, level1, level2, level3);
        await AppDbContext.SaveChangesAsync();

        var parents = await repository.GetAllParentAsync(level3.Id);

        parents.Should().NotBeNull();
        parents.Should().HaveCount(4);
        parents!.Select(p => p.Level).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task GetAllParentAsync_WithRootCategory_Should_ReturnOnlySelf()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        AppDbContext.Categories.Add(root);
        await AppDbContext.SaveChangesAsync();

        var parents = await repository.GetAllParentAsync(root.Id);

        parents.Should().ContainSingle();
        parents!.First().Id.Should().Be(root.Id);
    }

    [Fact]
    public async Task GetAllParentAsync_WithNonExistentId_Should_ReturnEmpty()
    {
        var parents = await repository.GetAllParentAsync(Guid.NewGuid());

        parents.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllParentAsync_Should_ExcludeDeletedCategories()
    {
        var root = Category.Create("Root", "root", 0, true);
        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        level1.Delete();
        await AppDbContext.SaveChangesAsync();

        var parents = await repository.GetAllParentAsync(level2.Id);

        parents!.Should().NotContain(p => p.Id == level1.Id);
    }
    [Fact]
    public async Task GetChildrenAsync_WithComplexTree_Should_ReturnEntireSubtree()
    {
        var root = Category.Create("Electronics", "electronics", 0, true);
        var branch1 = Category.Create("Computers", "computers", 1, true, root.Id);
        var branch2 = Category.Create("Phones", "phones", 1, true, root.Id);
        var leaf1 = Category.Create("Laptops", "laptops", 2, true, branch1.Id);
        var leaf2 = Category.Create("Smartphones", "smartphones", 2, true, branch2.Id);

        AppDbContext.Categories.AddRange(root, branch1, branch2, leaf1, leaf2);
        await AppDbContext.SaveChangesAsync();

        var children = await repository.GetChildrenAsync(root.Id);

        children.Should().HaveCount(5);
        children.Select(c => c.Name).Should().Contain(new[]
        {
            "Electronics", "Computers", "Phones", "Laptops", "Smartphones"
        });
    }

    [Fact]
    public async Task GetChildrenAsync_WithLeafNode_Should_ReturnOnlySelf()
    {
        var leaf = Category.Create("Laptops", "laptops", 0, true);
        AppDbContext.Categories.Add(leaf);
        await AppDbContext.SaveChangesAsync();

        var children = await repository.GetChildrenAsync(leaf.Id);

        children.Should().ContainSingle();
        children.First().Id.Should().Be(leaf.Id);
    }

    [Fact]
    public async Task GetChildrenAsync_Should_OrderByLevel()
    {
        var root = Category.Create("Root", "root", 0, true);
        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

        AppDbContext.Categories.AddRange(root, level1, level2);
        await AppDbContext.SaveChangesAsync();

        var children = await repository.GetChildrenAsync(root.Id);

        children.Select(c => c.Level).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task GetChildrenAsync_Should_ExcludeDeletedCategories()
    {
        var root = Category.Create("Root", "root", 0, true);
        var child1 = Category.Create("Child1", "child1", 1, true, root.Id);
        var child2 = Category.Create("Child2", "child2", 1, true, root.Id);

        AppDbContext.Categories.AddRange(root, child1, child2);
        await AppDbContext.SaveChangesAsync();

        child1.Delete();
        await AppDbContext.SaveChangesAsync();

        var children = await repository.GetChildrenAsync(root.Id);

        children.Should().NotContain(c => c.Id == child1.Id);
        children.Should().Contain(c => c.Id == child2.Id);
    }

    [Fact]
    public async Task CompleteWorkflow_AddUpdateDelete_Should_WorkCorrectly()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        repository.Add(category);
        await AppDbContext.SaveChangesAsync();

        var exists = await repository.ExistsAsync(category.Id);
        exists.Should().BeTrue();

        category.UpdateDetails("Electronics Updated", null);
        repository.Update(category);
        await AppDbContext.SaveChangesAsync();

        var updated = await repository.FindByIdAsync(category.Id);
        updated!.Name.Should().Be("Electronics Updated");

        repository.Remove(category);
        await AppDbContext.SaveChangesAsync();

        var deleted = await repository.FindByIdAsync(category.Id);
        deleted.Should().BeNull();
    }
}