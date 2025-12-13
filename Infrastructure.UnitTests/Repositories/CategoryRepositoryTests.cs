//using CatalogService.Domain.Entities;
//using CatalogService.Infrastructure.Persistence.Contexts;
//using CatalogService.Infrastructure.Persistence.Repositories;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Moq;

//namespace Infrastructure.UnitTests.Repositories;

//public sealed class CategoryRepositoryTests
//{
//    private readonly Mock<ApplicationDbContext> _mockContext;
//    private readonly Mock<DbSet<Category>> _mockDbSet;
//    private readonly CategoryRepository _repository;

//    public CategoryRepositoryTests()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .Options;

//        _mockContext = new Mock<ApplicationDbContext>(options);
//        _mockDbSet = new Mock<DbSet<Category>>();

//        _mockContext.Setup(c => c.Set<Category>()).Returns(_mockDbSet.Object);

//        _repository = new CategoryRepository(_mockContext.Object);
//    }
//    [Fact]
//    public void Add_WithValidCategory_Should_CallDbSetAdd()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        var id = _repository.Add(category);

//        id.Should().Be(category.Id);
//        _mockDbSet.Verify(m => m.Add(category), Times.Once);
//    }

//    [Fact]
//    public void Add_WithNullCategory_Should_ThrowArgumentNullException()
//    {
//        Action act = () => _repository.Add(null!);

//        act.Should().Throw<ArgumentNullException>();
//    }

//    [Fact]
//    public void AddRange_WithValidCategories_Should_CallDbSetAddRange()
//    {
//        var categories = new[]
//        {
//            Category.Create("Electronics", "electronics", 0, true),
//            Category.Create("Clothing", "clothing", 0, true)
//        };

//        _repository.AddRange(categories);

//        _mockDbSet.Verify(m => m.AddRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
//    }

//    [Fact]
//    public void AddRange_WithNullCollection_Should_ThrowArgumentNullException()
//    {
//        Action act = () => _repository.AddRange(null!);

//        act.Should().Throw<ArgumentNullException>();
//    }

//    [Fact]
//    public void Update_WithValidCategory_Should_CallDbSetUpdate()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.Update(category);

//        _mockDbSet.Verify(m => m.Update(category), Times.Once);
//    }

//    [Fact]
//    public void Update_WithNullCategory_Should_ThrowArgumentNullException()
//    {
//        Action act = () => _repository.Update(null!);

//        act.Should().Throw<ArgumentNullException>();
//    }

//    [Fact]
//    public void UpdateRange_WithValidCategories_Should_CallDbSetUpdateRange()
//    {
//        var categories = new[]
//        {
//            Category.Create("Cat1", "cat1", 0, true),
//            Category.Create("Cat2", "cat2", 0, true)
//        };

//        _repository.UpdateRange(categories);

//        _mockDbSet.Verify(m => m.UpdateRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
//    }

//    [Fact]
//    public void UpdateRange_WithNullCollection_Should_ThrowArgumentNullException()
//    {
//        Action act = () => _repository.UpdateRange(null!);

//        act.Should().Throw<ArgumentNullException>();
//    }

//    [Fact]
//    public void Remove_WithValidCategory_Should_CallDbSetRemove()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.Remove(category);

//        _mockDbSet.Verify(m => m.Remove(category), Times.Once);
//    }

//    [Fact]
//    public void Remove_WithNullCategory_Should_ThrowArgumentNullException()
//    {
//        Action act = () => _repository.Remove(null!);

//        act.Should().Throw<ArgumentNullException>();
//    }

//    [Fact]
//    public void RemoveRange_WithValidCategories_Should_CallDbSetRemoveRange()
//    {
//        var categories = new[]
//        {
//            Category.Create("Cat1", "cat1", 0, true),
//            Category.Create("Cat2", "cat2", 0, true)
//        };

//        _repository.RemoveRange(categories);

//        _mockDbSet.Verify(m => m.RemoveRange(It.IsAny<IEnumerable<Category>>()), Times.Once);
//    }

//    [Fact]
//    public void RemoveRange_WithNullCollection_Should_ThrowArgumentNullException()
//    {
//        Action act = () => _repository.RemoveRange(null!);

//        act.Should().Throw<ArgumentNullException>();
//    }

//    [Fact]
//    public void Add_Should_NotCallSaveChanges()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.Add(category);

//        _mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
//    }

//    [Fact]
//    public void Update_Should_NotCallSaveChanges()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.Update(category);

//        _mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
//    }

//    [Fact]
//    public void Remove_Should_NotCallSaveChanges()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.Remove(category);

//        _mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
//    }

//    [Fact]
//    public void Add_CalledMultipleTimes_Should_CallDbSetAddEachTime()
//    {
//        var cat1 = Category.Create("Cat1", "cat1", 0, true);
//        var cat2 = Category.Create("Cat2", "cat2", 0, true);
//        var cat3 = Category.Create("Cat3", "cat3", 0, true);

//        _repository.Add(cat1);
//        _repository.Add(cat2);
//        _repository.Add(cat3);

//        _mockDbSet.Verify(m => m.Add(It.IsAny<Category>()), Times.Exactly(3));
//    }

//    [Fact]
//    public void AddRange_WithEmptyCollection_Should_NotThrow()
//    {
//        var act = () => _repository.AddRange(Enumerable.Empty<Category>());

//        act.Should().NotThrow();
//    }

//    [Fact]
//    public void RemoveRange_WithSingleEntity_Should_StillCallRemoveRange()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        _repository.RemoveRange(new[] { category });

//        _mockDbSet.Verify(m => m.RemoveRange(It.Is<IEnumerable<Category>>(c => c.Count() == 1)), Times.Once);
//    }
//}