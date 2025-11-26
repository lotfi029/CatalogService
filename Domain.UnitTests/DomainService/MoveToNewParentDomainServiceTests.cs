using CatalogService.Domain.DomainService;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Errors;
using CatalogService.Domain.IRepositories;
using FluentAssertions;
using Moq;

namespace Domain.UnitTests.DomainService;

public sealed class MoveToNewParentDomainServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly CategoryDomainService _sut;

    public MoveToNewParentDomainServiceTests()
    {
        _mockRepository = new Mock<ICategoryRepository>();
        _sut = new CategoryDomainService(_mockRepository.Object);
    }

    [Fact]
    public async Task MoveToNewParent_WithNonExistentCategory_Should_ReturnNotFound()
    {
        var categoryId = Guid.NewGuid();
        var parent = Category.Create("Parent", "parent", 0, true);

        _mockRepository
            .Setup(x => x.GetChildrenAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Category>?)null!);

        var result = await _sut.MoveToNewParent(categoryId, parent);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NotFound(categoryId));
    }

    [Fact]
    public async Task MoveToNewParent_WithEmptyCategoryTree_Should_ReturnNotFound()
    {
        var categoryId = Guid.NewGuid();
        var parent = Category.Create("Parent", "parent", 0, true);

        _mockRepository
            .Setup(x => x.GetChildrenAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        var result = await _sut.MoveToNewParent(categoryId, parent);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NotFound(categoryId));
    }

    [Fact]
    public async Task MoveToNewParent_WhenMovingToDescendant_Should_ReturnInvalidChildToMoving()
    {
        var rootId = Guid.NewGuid();
        var root = Category.Create("Root", "root", 0, true);
        var child = Category.Create("Child", "child", 1, true, rootId);
        var grandchild = Category.Create("Grandchild", "grandchild", 2, true, child.Id);

        var categoryTree = new List<Category> { root, child, grandchild };

        _mockRepository
            .Setup(x => x.GetChildrenAsync(rootId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryTree);

        var result = await _sut.MoveToNewParent(rootId, child);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.InvalidChildToMoving);
    }

    [Fact]
    public async Task MoveToNewParent_WithValidMove_Should_UpdateCategoryAndReturnTree()
    {
        var category = Category.Create("Category", "category", 1, true, Guid.NewGuid());
        var newParent = Category.Create("NewParent", "new-parent", 0, true);

        var categoryTree = new List<Category> { category };

        _mockRepository
            .Setup(x => x.GetChildrenAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryTree);

        var result = await _sut.MoveToNewParent(category.Id, newParent);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(1);

        var movedCategory = result.Value!.First();
        movedCategory.ParentId.Should().Be(newParent.Id);
        movedCategory.Level.Should().Be(1);
        movedCategory.Path.Should().Be("new-parent/category");
    }

    [Fact]
    public async Task MoveToNewParent_WithChildren_Should_UpdateAllDescendants()
    {
        var root = Category.Create("Root", "root", 1, true, Guid.NewGuid());
        var child1 = Category.Create("Child1", "child1", 2, true, root.Id);
        var child2 = Category.Create("Child2", "child2", 2, true, root.Id);
        var grandchild = Category.Create("Grandchild", "grandchild", 3, true, child1.Id);

        var categoryTree = new List<Category> { root, child1, child2, grandchild };
        var newParent = Category.Create("NewParent", "new-parent", 0, true);

        _mockRepository
            .Setup(x => x.GetChildrenAsync(root.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryTree);

        var result = await _sut.MoveToNewParent(root.Id, newParent);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(4);

        var movedRoot = result.Value!.First(c => c.Id == root.Id);
        movedRoot.ParentId.Should().Be(newParent.Id);
        movedRoot.Level.Should().Be(1);
        movedRoot.Path.Should().Be("new-parent/root");

        var movedChild1 = result.Value!.First(c => c.Id == child1.Id);
        movedChild1.Level.Should().Be(2);
        movedChild1.Path.Should().Be("new-parent/root/child1");

        var movedChild2 = result.Value!.First(c => c.Id == child2.Id);
        movedChild2.Level.Should().Be(2);
        movedChild2.Path.Should().Be("new-parent/root/child2");

        var movedGrandchild = result.Value!.First(c => c.Id == grandchild.Id);
        movedGrandchild.Level.Should().Be(3);
        movedGrandchild.Path.Should().Be("new-parent/root/child1/grandchild");
    }

    [Fact]
    public async Task MoveToNewParent_WithDeepHierarchy_Should_UpdateAllLevels()
    {
        var root = Category.Create("Root", "root", 2, true, Guid.NewGuid(), path: "parent1/parent2");
        var level1 = Category.Create("Level1", "level1", 3, true, root.Id, path: "parent1/parent2/root");
        var level2 = Category.Create("Level2", "level2", 4, true, level1.Id, path: "parent1/parent2/root/level1");

        var categoryTree = new List<Category> { root, level1, level2 };
        var newParent = Category.Create("NewParent", "new-parent", 0, true);

        _mockRepository
            .Setup(x => x.GetChildrenAsync(root.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryTree);

        var result = await _sut.MoveToNewParent(root.Id, newParent);

        result.IsSuccess.Should().BeTrue();

        var movedRoot = result.Value!.First(c => c.Id == root.Id);
        movedRoot.Level.Should().Be(1);
        movedRoot.Path.Should().Be("new-parent/root");

        var movedLevel1 = result.Value!.First(c => c.Id == level1.Id);
        movedLevel1.Level.Should().Be(2);
        movedLevel1.Path.Should().Be("new-parent/root/level1");

        var movedLevel2 = result.Value!.First(c => c.Id == level2.Id);
        movedLevel2.Level.Should().Be(3);
        movedLevel2.Path.Should().Be("new-parent/root/level1/level2");
    }

    [Fact]
    public async Task MoveToNewParent_WithInconsistentTree_Should_ReturnInconsistentTreeStructure()
    {
        var root = Category.Create("Root", "root", 0, true);

        var orphan = Category.Create("Orphan", "orphan", 2, true, Guid.NewGuid());

        var categoryTree = new List<Category> { root, orphan };
        var newParent = Category.Create("NewParent", "new-parent", 0, true);

        _mockRepository
            .Setup(x => x.GetChildrenAsync(root.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryTree);

        var result = await _sut.MoveToNewParent(root.Id, newParent);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.InconsistentTreeStructure);
    }

    [Fact]
    public async Task MoveToNewParent_WithMultipleBranches_Should_UpdateAllBranches()
    {
        var root = Category.Create("Root", "root", 0, true);
        var branch1 = Category.Create("Branch1", "branch1", 1, true, root.Id);
        var branch2 = Category.Create("Branch2", "branch2", 1, true, root.Id);
        var leaf1 = Category.Create("Leaf1", "leaf1", 2, true, branch1.Id);
        var leaf2 = Category.Create("Leaf2", "leaf2", 2, true, branch2.Id);

        var categoryTree = new List<Category> { root, branch1, branch2, leaf1, leaf2 };
        var newParent = Category.Create("NewParent", "new-parent", 0, true);

        _mockRepository
            .Setup(x => x.GetChildrenAsync(root.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryTree);

        var result = await _sut.MoveToNewParent(root.Id, newParent);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(5);

        result.Value!.All(c => c.Path!.StartsWith("new-parent/")).Should().BeTrue();
    }

    [Fact]
    public async Task MoveToNewParent_WithCancellationToken_Should_PassToRepository()
    {
        var category = Category.Create("Category", "category", 0, true);
        var parent = Category.Create("Parent", "parent", 0, true);
        var cts = new CancellationTokenSource();
        var ct = cts.Token;

        _mockRepository
            .Setup(x => x.GetChildrenAsync(category.Id, ct))
            .ReturnsAsync([category]);

        var result = await _sut.MoveToNewParent(category.Id, parent, ct);

        result.IsSuccess.Should().BeTrue();

        _mockRepository.Verify(
            x => x.GetChildrenAsync(category.Id, ct),
            Times.Once);
    }
}