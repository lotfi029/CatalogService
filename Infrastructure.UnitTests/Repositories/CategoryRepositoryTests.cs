using CatalogService.Domain.Entities;
using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence.Contexts;
using CatalogService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace Infrastructure.UnitTests.Repositories;

public sealed class CategoryRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly Mock<DbSet<Category>> _mockDbSet;
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options;
        _mockContext = new Mock<ApplicationDbContext>(options);
        _mockDbSet = new Mock<DbSet<Category>>();

        _mockContext.Setup(c => c.Set<Category>()).Returns(_mockDbSet.Object);
        _repository = new CategoryRepository(_mockContext.Object);
    }

    [Fact]
    public void Add_WithValidEntity_Should_CallDbSetAdd()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        var id = _repository.Add(category);

        id.Should().Be(category.Id);
        _mockDbSet.Verify(m => m.Add(category), Times.Once);
    }
    [Fact]
    public void Add_WithValidEntity_Should_ReturnEntityId()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        var id = _repository.Add(category);

        id.Should().NotBeEmpty();
        id.Should().Be(category.Id);
    }

    [Fact]
    public void Add_WithNullEntity_Should_ThrowArgumentNullException()
    {
        Action act = () => _repository.Add(null!);

        act.Should().Throw<ArgumentNullException>();
    }
    [Fact]
    public void AddRange_WithNullCollection_Should_ThrowArgumentNullException()
    {
        Action act = () => _repository.AddRange(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddRange_WithEmptyCollection_Should_NotThrow()
    {
        var act = () => _repository.AddRange(Enumerable.Empty<Category>());

        act.Should().NotThrow();
    }
    [Fact]
    public void Update_WithValidEntity_Should_CallDbSetUpdate()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        _repository.Update(category);

        _mockDbSet.Verify(m => m.Update(category), Times.Once);
    }

    [Fact]
    public void Update_WithNullEntity_Should_ThrowArgumentNullException()
    {
        Action act = () => _repository.Update(null!);

        act.Should().Throw<ArgumentNullException>();
    }
    [Fact]
    public void UpdateRange_WithValidEntities_Should_CallDbSetUpdateRange()
    {
        var categories = new[]
        {
            Category.Create("Cat1", "cat1", 0, true),
            Category.Create("Cat2", "cat2", 0, true)
        };

        _repository.UpdateRange(categories);

        _mockDbSet.Verify(m => m.UpdateRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
    }

    [Fact]
    public void UpdateRange_WithNullCollection_Should_ThrowArgumentNullException()
    {
        Action act = () => _repository.UpdateRange(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Remove_WithValidEntity_Should_CallDbSetRemove()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        _repository.Remove(category);

        _mockDbSet.Verify(m => m.Remove(category), Times.Once);
    }

    [Fact]
    public void Remove_WithNullEntity_Should_ThrowArgumentNullException()
    {
        Action act = () => _repository.Remove(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RemoveRange_WithValidEntities_Should_CallDbSetRemoveRange()
    {
        var categories = new[]
        {
            Category.Create("Cat1", "cat1", 0, true),
            Category.Create("Cat2", "cat2", 0, true)
        };

        _repository.RemoveRange(categories);

        _mockDbSet.Verify(m => m.RemoveRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
    }

    [Fact]
    public void RemoveRange_WithNullCollection_Should_ThrowArgumentNullException()
    {
        Action act = () => _repository.RemoveRange(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task ExecuteDeleteAsync_WithNullPredicate_Should_ThrowArgumentNullException()
    {
        var act = async () => await _repository.ExecuteDeleteAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ExistsAsync_ByPredicate_WithNullPredicate_Should_ThrowArgumentNullException()
    {
        var act = async () => await _repository.ExistsAsync((Expression<Func<Category, bool>>)null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    [Fact]
    public async Task FindAsync_WithNullPredicate_Should_ThrowArgumentNullException()
    {
        var act = async () => await _repository.FindAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    [Fact]
    public void Add_Should_NotCallSaveChanges()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        _repository.Add(category);

        _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Update_Should_NotCallSaveChanges()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        _repository.Update(category);

        _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Remove_Should_NotCallSaveChanges()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        _repository.Remove(category);

        _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void AddRange_WithMultipleEntities_Should_CallAddRangeOnce()
    {
        var categories = new[]
        {
            Category.Create("Cat1", "cat1", 0, true),
            Category.Create("Cat2", "cat2", 0, true),
            Category.Create("Cat3", "cat3", 0, true)
        };

        _repository.AddRange(categories);

        _mockDbSet.Verify(m => m.AddRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
    }
    [Fact]
    public void UpdateRange_WithMultipleEntities_Should_CallUpdateRangeOnce()
    {
        var categories = new[]
        {
            Category.Create("Cat1", "cat1", 0, true),
            Category.Create("Cat2", "cat2", 0, true)
        };

        _repository.UpdateRange(categories);

        _mockDbSet.Verify(m => m.UpdateRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
    }

    [Fact]
    public void RemoveRange_WithMultipleEntities_Should_CallRemoveRangeOnce()
    {
        var categories = new[]
        {
            Category.Create("Cat1", "cat1", 0, true),
            Category.Create("Cat2", "cat2", 0, true)
        };

        _repository.RemoveRange(categories);

        _mockDbSet.Verify(m => m.RemoveRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
    }

    [Fact]
    public void Add_CalledMultipleTimes_Should_CallDbSetAddEachTime()
    {
        var cat1 = Category.Create("Cat1", "cat1", 0, true);
        var cat2 = Category.Create("Cat2", "cat2", 0, true);

        _repository.Add(cat1);
        _repository.Add(cat2);

        _mockDbSet.Verify(m => m.Add(It.IsAny<Category>()), Times.Exactly(2));
    }

    [Fact]
    public void AddRange_WithSingleEntity_Should_StillCallAddRange()
    {
        var category = Category.Create("Electronics", "electronics", 0, true);

        _repository.AddRange(new[] { category });

        _mockDbSet.Verify(m => m.AddRange(It.Is<IEnumerable<Category>>(c => c.Count() == 1)), Times.Once);
    }

}