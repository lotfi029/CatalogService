//using CatalogService.Infrastructure.Persistence.Repositories;
//namespace CatalogService.IntegrationTests.Repositories;
//public class CategoryRepositoryIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTests(factory)
//{
//    CategoryRepository repository = null!;

//    public async override Task InitializeAsync()
//    {
//        await base.InitializeAsync();
//        repository = new CategoryRepository(AppDbContext);
//    }

//    [Fact]
//    public async Task Add_WithValidEntity_Should_PersistToDatabase()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        repository.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var saved = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstOrDefaultAsync(c => c.Id == category.Id);

//        saved.Should().NotBeNull();
//        saved!.Name.Should().Be("Electronics");
//    }

//    [Fact]
//    public async Task Update_WithValidEntity_Should_PersistChangesToDatabase()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        category.UpdateDetails("Electronics Updated", "New description");
//        repository.Update(category);
//        await AppDbContext.SaveChangesAsync();

//        var updated = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstAsync(c => c.Id == category.Id);

//        updated.Name.Should().Be("Electronics Updated");
//        updated.Description.Should().Be("New description");
//    }

//    [Fact]
//    public async Task Remove_WithValidEntity_Should_DeleteFromDatabase()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        repository.Remove(category);
//        await AppDbContext.SaveChangesAsync();

//        var exists = await AppDbContext.Categories.AnyAsync(c => c.Id == category.Id);
//        exists.Should().BeFalse();
//    }

//    [Fact]
//    public async Task ExistsAsync_ById_WithExistingEntity_Should_ReturnTrue()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var exists = await repository.ExistsAsync(category.Id);

//        exists.Should().BeTrue();
//    }

//    [Fact]
//    public async Task FindByIdAsync_WithExistingEntity_Should_ReturnEntity()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var found = await repository.FindAsync(category.Id, null);

//        found.Should().NotBeNull();
//        found!.Name.Should().Be("Electronics");
//    }

//    [Fact]
//    public async Task FindAsync_WithPredicate_Should_ReturnMatchingEntity()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var found = await repository.FindAsync(c => c.Slug == "electronics");

//        found.Should().NotBeNull();
//        found!.Name.Should().Be("Electronics");
//    }

//    [Fact]
//    public async Task ExecuteDeleteAsync_WithPredicate_Should_DeleteMatchingEntities()
//    {
//        var cat1 = Category.Create("Cat1", "cat1", 0, false);
//        var cat2 = Category.Create("Cat2", "cat2", 0, false);
//        var cat3 = Category.Create("Cat3", "cat3", 0, true);

//        AppDbContext.Categories.AddRange(cat1, cat2, cat3);
//        await AppDbContext.SaveChangesAsync();

//        var deletedCount = await repository.ExecuteDeleteAsync(c => !c.IsActive);

//        deletedCount.Should().Be(2);

//        var remaining = await AppDbContext.Categories.CountAsync();
//        remaining.Should().Be(1);
//    }
//    [Fact]
//    public async Task GetAllParentAsync_WithDeepHierarchy_Should_ReturnAllAncestors()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        var level1 = Category.Create("Computers", "computers", 1, true, root.Id, parentPath: "electronics");
//        var level2 = Category.Create("Laptops", "laptops", 2, true, level1.Id, parentPath: "electronics/computers");
//        var level3 = Category.Create("Gaming", "gaming", 3, true, level2.Id, parentPath: "electronics/computers/laptops");

//        AppDbContext.Categories.AddRange(root, level1, level2, level3);
//        await AppDbContext.SaveChangesAsync();

//        var parents = await repository.GetAllParentAsync(level3.Id);

//        parents.Should().NotBeNull();
//        parents.Should().HaveCount(4);
//        parents!.Select(p => p.Level).Should().BeInAscendingOrder();
//    }

//    [Fact]
//    public async Task GetAllParentAsync_WithRootCategory_Should_ReturnOnlySelf()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(root);
//        await AppDbContext.SaveChangesAsync();

//        var parents = await repository.GetAllParentAsync(root.Id);

//        parents.Should().ContainSingle();
//        parents!.First().Id.Should().Be(root.Id);
//    }

//    [Fact]
//    public async Task GetAllParentAsync_WithNonExistentId_Should_ReturnEmpty()
//    {
//        var parents = await repository.GetAllParentAsync(Guid.NewGuid());

//        parents.Should().BeEmpty();
//    }

//    [Fact]
//    public async Task GetAllParentAsync_Should_ExcludeDeletedCategories()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
//        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

//        AppDbContext.Categories.AddRange(root, level1, level2);
//        await AppDbContext.SaveChangesAsync();

//        level1.Deleted();
//        await AppDbContext.SaveChangesAsync();

//        var parents = await repository.GetAllParentAsync(level2.Id);

//        parents!.Should().NotContain(p => p.Id == level1.Id);
//    }
//    [Fact]
//    public async Task GetChildrenAsync_WithComplexTree_Should_ReturnEntireSubtree()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        var branch1 = Category.Create("Computers", "computers", 1, true, root.Id);
//        var branch2 = Category.Create("Phones", "phones", 1, true, root.Id);
//        var leaf1 = Category.Create("Laptops", "laptops", 2, true, branch1.Id);
//        var leaf2 = Category.Create("Smartphones", "smartphones", 2, true, branch2.Id);

//        AppDbContext.Categories.AddRange(root, branch1, branch2, leaf1, leaf2);
//        await AppDbContext.SaveChangesAsync();

//        var children = await repository.GetChildrenAsync(root.Id);

//        children.Should().HaveCount(5);
//        children.Select(c => c.Name).Should().Contain(
//        [
//            "Electronics", "Computers", "Phones", "Laptops", "Smartphones"
//        ]);
//    }

//    [Fact]
//    public async Task GetChildrenAsync_WithLeafNode_Should_ReturnOnlySelf()
//    {
//        var leaf = Category.Create("Laptops", "laptops", 0, true);
//        AppDbContext.Categories.Add(leaf);
//        await AppDbContext.SaveChangesAsync();

//        var children = await repository.GetChildrenAsync(leaf.Id);

//        children.Should().ContainSingle();
//        children.First().Id.Should().Be(leaf.Id);
//    }

//    [Fact]
//    public async Task GetChildrenAsync_Should_OrderByLevel()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
//        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

//        AppDbContext.Categories.AddRange(root, level1, level2);
//        await AppDbContext.SaveChangesAsync();

//        var children = await repository.GetChildrenAsync(root.Id);

//        children.Select(c => c.Level).Should().BeInAscendingOrder();
//    }

//    [Fact]
//    public async Task GetChildrenAsync_Should_ExcludeDeletedCategories()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var child1 = Category.Create("Child1", "child1", 1, true, root.Id);
//        var child2 = Category.Create("Child2", "child2", 1, true, root.Id);

//        AppDbContext.Categories.AddRange(root, child1, child2);
//        await AppDbContext.SaveChangesAsync();

//        child1.Deleted();
//        await AppDbContext.SaveChangesAsync();

//        var children = await repository.GetChildrenAsync(root.Id);

//        children.Should().NotContain(c => c.Id == child1.Id);
//        children.Should().Contain(c => c.Id == child2.Id);
//    }

//    [Fact]
//    public async Task CompleteWorkflow_AddUpdateDelete_Should_WorkCorrectly()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        repository.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var exists = await repository.ExistsAsync(category.Id);
//        exists.Should().BeTrue();

//        category.UpdateDetails("Electronics Updated", null);
//        repository.Update(category);
//        await AppDbContext.SaveChangesAsync();

//        var updated = await repository.FindAsync(category.Id, null);
//        updated!.Name.Should().Be("Electronics Updated");

//        repository.Remove(category);
//        await AppDbContext.SaveChangesAsync();

//        var deleted = await repository.FindAsync(category.Id, null);
//        deleted.Should().BeNull();
//    }
//}


//public class CategoryRepositoryTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTests(factory)
//{
//    private CategoryRepository _repository { get; set; } = null!;

    
//    public override async Task InitializeAsync()
//    {
//        await base.InitializeAsync();
//        _repository = new CategoryRepository(AppDbContext);
//    }
//    [Fact]
//    public async Task Add_WithValidCategory_Should_PersistToDatabase()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var saved = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstOrDefaultAsync(c => c.Id == category.Id);

//        saved.Should().NotBeNull();
//        saved!.Name.Should().Be("Electronics");
//        saved.Slug.Should().Be("electronics");
//        saved.Level.Should().Be(0);
//    }

//    [Fact]
//    public async Task Add_WithCompleteData_Should_PersistAllProperties()
//    {
//        var category = Category.Create(
//            name: "Electronics",
//            slug: "electronics",
//            level: 0,
//            isActive: true,
//            description: "Electronic products");

//        _repository.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var saved = await AppDbContext.Categories.FindAsync(category.Id);

//        saved.Should().NotBeNull();
//        saved!.Name.Should().Be("Electronics");
//        saved.Description.Should().Be("Electronic products");
//        saved.Path.Should().Be("electronics");
//        saved.IsActive.Should().BeTrue();
//    }

//    [Fact]
//    public async Task AddRange_WithMultipleCategories_Should_PersistAllToDatabase()
//    {
//        var categories = new[]
//        {
//            Category.Create("Electronics", "electronics", 0, true),
//            Category.Create("Clothing", "clothing", 0, true),
//            Category.Create("Books", "books", 0, true)
//        };

//        _repository.AddRange(categories);
//        await AppDbContext.SaveChangesAsync();

//        var count = await AppDbContext.Categories.CountAsync();
//        count.Should().Be(3);

//        var slugs = await AppDbContext.Categories
//            .Select(c => c.Slug)
//            .ToListAsync();

//        slugs.Should().Contain(new[] { "electronics", "clothing", "books" });
//    }

//    [Fact]
//    public async Task Add_WithDuplicateSlug_Should_ThrowException()
//    {
//        var category1 = Category.Create("Electronics", "electronics", 0, true);
//        var category2 = Category.Create("Electronics 2", "electronics", 0, true);

//        _repository.Add(category1);
//        await AppDbContext.SaveChangesAsync();

//        _repository.Add(category2);
//        var act = async () => await AppDbContext.SaveChangesAsync();

//        await act.Should().ThrowAsync<DbUpdateException>();
//    }

//    [Fact]
//    public async Task Update_WithValidCategory_Should_PersistChanges()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        category.UpdateDetails("Electronics Updated", "New description");
//        _repository.Update(category);
//        await AppDbContext.SaveChangesAsync();

//        var updated = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstAsync(c => c.Id == category.Id);

//        updated.Name.Should().Be("Electronics Updated");
//        updated.Description.Should().Be("New description");
//    }

//    [Fact]
//    public async Task UpdateRange_WithMultipleCategories_Should_PersistAllChanges()
//    {
//        var cat1 = Category.Create("Cat1", "cat1", 0, true);
//        var cat2 = Category.Create("Cat2", "cat2", 0, true);

//        AppDbContext.Categories.AddRange(cat1, cat2);
//        await AppDbContext.SaveChangesAsync();

//        cat1.UpdateDetails("Cat1 Updated", null);
//        cat2.UpdateDetails("Cat2 Updated", null);

//        _repository.UpdateRange(new[] { cat1, cat2 });
//        await AppDbContext.SaveChangesAsync();

//        var updated1 = await AppDbContext.Categories.FindAsync(cat1.Id);
//        var updated2 = await AppDbContext.Categories.FindAsync(cat2.Id);

//        updated1!.Name.Should().Be("Cat1 Updated");
//        updated2!.Name.Should().Be("Cat2 Updated");
//    }

//    [Fact]
//    public async Task Update_Should_UpdateAuditFields()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        await Task.Delay(100);

//        category.UpdateDetails("Updated", null);
//        _repository.Update(category);
//        await AppDbContext.SaveChangesAsync();

//        var updated = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstAsync(c => c.Id == category.Id);

//        updated.LastUpdatedAt.Should().NotBeNull();
//    }

//    [Fact]
//    public async Task Remove_WithValidCategory_Should_DeleteFromDatabase()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        _repository.Remove(category);
//        await AppDbContext.SaveChangesAsync();

//        var exists = await AppDbContext.Categories.AnyAsync(c => c.Id == category.Id);
//        exists.Should().BeFalse();
//    }

//    [Fact]
//    public async Task RemoveRange_WithMultipleCategories_Should_DeleteAllFromDatabase()
//    {
//        var cat1 = Category.Create("Cat1", "cat1", 0, true);
//        var cat2 = Category.Create("Cat2", "cat2", 0, true);
//        var cat3 = Category.Create("Cat3", "cat3", 0, true);

//        AppDbContext.Categories.AddRange(cat1, cat2, cat3);
//        await AppDbContext.SaveChangesAsync();

//        _repository.RemoveRange(new[] { cat1, cat2 });
//        await AppDbContext.SaveChangesAsync();

//        var count = await AppDbContext.Categories.CountAsync();
//        count.Should().Be(1);

//        var remaining = await AppDbContext.Categories.FirstAsync();
//        remaining.Id.Should().Be(cat3.Id);
//    }
//    [Fact]
//    public async Task ExecuteDeleteAsync_WithMatchingPredicate_Should_DeleteEntities()
//    {
//        var cat1 = Category.Create("Cat1", "cat1", 0, false);
//        var cat2 = Category.Create("Cat2", "cat2", 0, false);
//        var cat3 = Category.Create("Cat3", "cat3", 0, true);

//        AppDbContext.Categories.AddRange(cat1, cat2, cat3);
//        await AppDbContext.SaveChangesAsync();

//        var deletedCount = await _repository.ExecuteDeleteAsync(c => !c.IsActive);

//        deletedCount.Should().Be(2);

//        var remaining = await AppDbContext.Categories.CountAsync();
//        remaining.Should().Be(1);

//        var active = await AppDbContext.Categories.FirstAsync();
//        active.IsActive.Should().BeTrue();
//    }

//    [Fact]
//    public async Task ExecuteDeleteAsync_WithNoMatches_Should_ReturnZero()
//    {
//        var category = Category.Create("Cat1", "cat1", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var deletedCount = await _repository.ExecuteDeleteAsync(c => c.Name == "NonExistent");

//        deletedCount.Should().Be(0);

//        var count = await AppDbContext.Categories.CountAsync();
//        count.Should().Be(1);
//    }


//    [Fact]
//    public async Task ExistsAsync_ById_WithExistingCategory_Should_ReturnTrue()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var exists = await _repository.ExistsAsync(category.Id);

//        exists.Should().BeTrue();
//    }

//    [Fact]
//    public async Task ExistsAsync_ById_WithNonExistentId_Should_ReturnFalse()
//    {
//        var exists = await _repository.ExistsAsync(Guid.NewGuid());

//        exists.Should().BeFalse();
//    }

//    [Fact]
//    public async Task ExistsAsync_ByPredicate_WithMatch_Should_ReturnTrue()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var exists = await _repository.ExistsAsync(c => c.Slug == "electronics");

//        exists.Should().BeTrue();
//    }

//    [Fact]
//    public async Task ExistsAsync_ByPredicate_WithNoMatch_Should_ReturnFalse()
//    {
//        var exists = await _repository.ExistsAsync(c => c.Slug == "nonexistent");

//        exists.Should().BeFalse();
//    }

//    // ===================================================================
//    // FIND BY ID ASYNC - Integration Tests
//    // ===================================================================

//    [Fact]
//    public async Task FindByIdAsync_WithExistingId_Should_ReturnEntity()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var found = await _repository.FindAsync(category.Id, null);

//        found.Should().NotBeNull();
//        found!.Name.Should().Be("Electronics");
//    }

//    [Fact]
//    public async Task FindByIdAsync_WithNonExistentId_Should_ReturnNull()
//    {
//        var found = await _repository.FindAsync(Guid.NewGuid(), null);

//        found.Should().BeNull();
//    }

//    // ===================================================================
//    // FIND ASYNC - Integration Tests
//    // ===================================================================

//    [Fact]
//    public async Task FindAsync_WithMatchingPredicate_Should_ReturnEntity()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var found = await _repository.FindAsync(c => c.Slug == "electronics");

//        found.Should().NotBeNull();
//        found!.Name.Should().Be("Electronics");
//    }

//    [Fact]
//    public async Task FindAsync_WithNoMatch_Should_ReturnNull()
//    {
//        var found = await _repository.FindAsync(c => c.Slug == "nonexistent");

//        found.Should().BeNull();
//    }

//    [Fact]
//    public async Task FindAsync_Should_ReturnUntrackedEntity()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var found = await _repository.FindAsync(c => c.Id == category.Id);

//        found.Should().NotBeNull();

//        var entry = AppDbContext.Entry(found!);
//        entry.State.Should().Be(EntityState.Detached);
//    }

//    // ===================================================================
//    // GET ALL PARENT ASYNC - Integration Tests
//    // ===================================================================

//    [Fact]
//    public async Task GetAllParentAsync_WithDeepHierarchy_Should_ReturnAllAncestors()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        var level1 = Category.Create("Computers", "computers", 1, true, root.Id, parentPath: "electronics");
//        var level2 = Category.Create("Laptops", "laptops", 2, true, level1.Id, parentPath: "electronics/computers");
//        var level3 = Category.Create("Gaming", "gaming", 3, true, level2.Id, parentPath: "electronics/computers/laptops");

//        AppDbContext.Categories.AddRange(root, level1, level2, level3);
//        await AppDbContext.SaveChangesAsync();

//        var parents = await _repository.GetAllParentAsync(level3.Id);

//        parents.Should().NotBeNull();
//        parents.Should().HaveCount(4);
//        parents!.Select(p => p.Level).Should().BeInAscendingOrder();
//        parents.Select(p => p.Name).Should().Contain(new[] { "Gaming", "Laptops", "Computers", "Electronics" });
//    }

//    [Fact]
//    public async Task GetAllParentAsync_WithRootCategory_Should_ReturnOnlySelf()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        AppDbContext.Categories.Add(root);
//        await AppDbContext.SaveChangesAsync();

//        var parents = await _repository.GetAllParentAsync(root.Id);

//        parents.Should().ContainSingle();
//        parents!.First().Id.Should().Be(root.Id);
//    }

//    [Fact]
//    public async Task GetAllParentAsync_WithNonExistentId_Should_ReturnEmpty()
//    {
//        var parents = await _repository.GetAllParentAsync(Guid.NewGuid());

//        parents.Should().BeEmpty();
//    }

//    [Fact]
//    public async Task GetAllParentAsync_Should_ExcludeDeletedCategories()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
//        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

//        AppDbContext.Categories.AddRange(root, level1, level2);
//        await AppDbContext.SaveChangesAsync();

//        level1.Deleted();
//        await AppDbContext.SaveChangesAsync();

//        var parents = await _repository.GetAllParentAsync(level2.Id);

//        parents!.Should().NotContain(p => p.Id == level1.Id);
//    }

//    [Fact]
//    public async Task GetAllParentAsync_Should_OrderByLevel()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
//        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

//        AppDbContext.Categories.AddRange(root, level1, level2);
//        await AppDbContext.SaveChangesAsync();

//        var parents = await _repository.GetAllParentAsync(level2.Id);

//        parents!.Select(p => p.Level).Should().BeInAscendingOrder();
//        parents.First().Level.Should().Be(0);
//        parents.Last().Level.Should().Be(2);
//    }
//    [Fact]
//    public async Task GetChildrenAsync_WithComplexTree_Should_ReturnEntireSubtree()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        var branch1 = Category.Create("Computers", "computers", 1, true, root.Id);
//        var branch2 = Category.Create("Phones", "phones", 1, true, root.Id);
//        var leaf1 = Category.Create("Laptops", "laptops", 2, true, branch1.Id);
//        var leaf2 = Category.Create("Desktops", "desktops", 2, true, branch1.Id);
//        var leaf3 = Category.Create("Smartphones", "smartphones", 2, true, branch2.Id);

//        AppDbContext.Categories.AddRange(root, branch1, branch2, leaf1, leaf2, leaf3);
//        await AppDbContext.SaveChangesAsync();

//        var children = await _repository.GetChildrenAsync(root.Id);

//        children.Should().HaveCount(6);
//        children.Select(c => c.Name).Should().Contain(new[]
//        {
//            "Electronics", "Computers", "Phones", "Laptops", "Desktops", "Smartphones"
//        });
//    }

//    [Fact]
//    public async Task GetChildrenAsync_WithLeafNode_Should_ReturnOnlySelf()
//    {
//        var leaf = Category.Create("Laptops", "laptops", 0, true);
//        AppDbContext.Categories.Add(leaf);
//        await AppDbContext.SaveChangesAsync();

//        var children = await _repository.GetChildrenAsync(leaf.Id);

//        children.Should().ContainSingle();
//        children.First().Id.Should().Be(leaf.Id);
//    }

//    [Fact]
//    public async Task GetChildrenAsync_WithNonExistentId_Should_ReturnEmpty()
//    {
//        var children = await _repository.GetChildrenAsync(Guid.NewGuid());

//        children.Should().BeEmpty();
//    }

//    [Fact]
//    public async Task GetChildrenAsync_Should_OrderByLevel()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
//        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);
//        var level3 = Category.Create("Level3", "level3", 3, true, level2.Id);

//        AppDbContext.Categories.AddRange(root, level1, level2, level3);
//        await AppDbContext.SaveChangesAsync();

//        var children = await _repository.GetChildrenAsync(root.Id);

//        children.Select(c => c.Level).Should().BeInAscendingOrder();
//    }

//    [Fact]
//    public async Task GetChildrenAsync_Should_ExcludeDeletedCategories()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var child1 = Category.Create("Child1", "child1", 1, true, root.Id);
//        var child2 = Category.Create("Child2", "child2", 1, true, root.Id);

//        AppDbContext.Categories.AddRange(root, child1, child2);
//        await AppDbContext.SaveChangesAsync();

//        child1.Deleted();
//        await AppDbContext.SaveChangesAsync();

//        var children = await _repository.GetChildrenAsync(root.Id);

//        children.Should().NotContain(c => c.Id == child1.Id);
//        children.Should().Contain(c => c.Id == child2.Id);
//    }

//    [Fact]
//    public async Task GetChildrenAsync_WithMultipleBranches_Should_ReturnAllBranches()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        var branch1 = Category.Create("Computers", "computers", 1, true, root.Id);
//        var branch2 = Category.Create("Phones", "phones", 1, true, root.Id);
//        var branch3 = Category.Create("Tablets", "tablets", 1, true, root.Id);

//        AppDbContext.Categories.AddRange(root, branch1, branch2, branch3);
//        await AppDbContext.SaveChangesAsync();

//        var children = await _repository.GetChildrenAsync(root.Id);

//        children.Should().HaveCount(4);
//        children.Count(c => c.Level == 1).Should().Be(3);
//    }

//    // ===================================================================
//    // COMPLEX SCENARIOS - Integration Tests
//    // ===================================================================

//    [Fact]
//    public async Task CompleteWorkflow_AddUpdateRemove_Should_WorkCorrectly()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var exists = await _repository.ExistsAsync(category.Id);
//        exists.Should().BeTrue();

//        category.UpdateDetails("Electronics Updated", "New description");
//        _repository.Update(category);
//        await AppDbContext.SaveChangesAsync();

//        var updated = await _repository.FindAsync(category.Id, null);
//        updated!.Name.Should().Be("Electronics Updated");

//        _repository.Remove(category);
//        await AppDbContext.SaveChangesAsync();

//        var deleted = await _repository.FindAsync(category.Id, null);
//        deleted.Should().BeNull();
//    }

//    [Fact]
//    public async Task GetAllParentAsync_And_GetChildrenAsync_Should_BeConsistent()
//    {
//        var root = Category.Create("Root", "root", 0, true);
//        var level1 = Category.Create("Level1", "level1", 1, true, root.Id);
//        var level2 = Category.Create("Level2", "level2", 2, true, level1.Id);

//        AppDbContext.Categories.AddRange(root, level1, level2);
//        await AppDbContext.SaveChangesAsync();

//        var parents = await _repository.GetAllParentAsync(level2.Id);
//        var children = await _repository.GetChildrenAsync(root.Id);

//        parents.Should().HaveCount(3);
//        children.Should().HaveCount(3);

//        parents!.Select(p => p.Id).Should().BeEquivalentTo(children.Select(c => c.Id));
//    }

//    [Fact]
//    public async Task Repository_WithHierarchyOperations_Should_MaintainIntegrity()
//    {
//        var root = Category.Create("Electronics", "electronics", 0, true);
//        var computers = Category.Create("Computers", "computers", 1, true, root.Id);
//        var laptops = Category.Create("Laptops", "laptops", 2, true, computers.Id);

//        _repository.AddRange(new[] { root, computers, laptops });
//        await AppDbContext.SaveChangesAsync();

//        var parents = await _repository.GetAllParentAsync(laptops.Id);
//        parents.Should().HaveCount(3);

//        var children = await _repository.GetChildrenAsync(root.Id);
//        children.Should().HaveCount(3);

//        laptops.UpdateDetails("Gaming Laptops", "High performance");
//        _repository.Update(laptops);
//        await AppDbContext.SaveChangesAsync();

//        var updated = await _repository.FindAsync(laptops.Id, null);
//        updated!.Name.Should().Be("Gaming Laptops");

//        _repository.Remove(laptops);
//        await AppDbContext.SaveChangesAsync();

//        var childrenAfterDelete = await _repository.GetChildrenAsync(root.Id);
//        childrenAfterDelete.Should().HaveCount(2);
//    }

//    [Fact]
//    public async Task Repository_ConcurrentOperations_Should_WorkCorrectly()
//    {
//        var cat1 = Category.Create("Cat1", "cat1", 0, true);
//        var cat2 = Category.Create("Cat2", "cat2", 0, true);
//        var cat3 = Category.Create("Cat3", "cat3", 0, true);

//        _repository.Add(cat1);
//        await AppDbContext.SaveChangesAsync();

//        _repository.Add(cat2);
//        await AppDbContext.SaveChangesAsync();

//        _repository.Add(cat3);
//        await AppDbContext.SaveChangesAsync();

//        var count = await AppDbContext.Categories.CountAsync();
//        count.Should().Be(3);
//    }
//}
