using CatalogService.Domain.DomainService;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Errors;
using CatalogService.Domain.IRepositories;
using FluentAssertions;
using Moq;
using SharedKernel;
using System.Linq.Expressions;

namespace Domain.UnitTests.DomainService;

public sealed class CategoryDomainServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly CategoryDomainService _sut;
    const string _name = "Electronics";
    const string _slug = "electronics";
    const string _Description = "Electronics Products";
    const bool _isActive = true;
    const int _maxDepth = 100;
    readonly Guid _parentId = Guid.CreateVersion7();
    public CategoryDomainServiceTests()
    {
        _mockRepository = new Mock<ICategoryRepository>();
        _sut = new CategoryDomainService(_mockRepository.Object);
    }
    [Fact]
    public async Task CreateCateogryAsync_Should_ReturnError_WhenSlugNotUnique()
    {
        _mockRepository.Setup(
            x => x.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.CreateCategoryAsync(
            name:_name,
            slug:_slug,
            isActive: _isActive,
            description: _Description);

        Category Action() => result.Value!;

        result.Error.Should().Be(CategoryErrors.SlugAlreadyExist(_slug));
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        FluentActions.Invoking(Action)
            .Should()
            .Throw<ArgumentException>()
            .Which.Message.Should().Be("invalid success result");
    }
    [Fact]
    public async Task CreateCategoryAsync_Should_ReturnError_WhenParentIdNotNullAndNotFound()
    {
        _mockRepository
            .Setup(x => x.ExistsAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockRepository
            .Setup(x => x.ExistsAsync(_parentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.CreateCategoryAsync(
            name: _name,
            slug: _slug,
            isActive: _isActive,
            parentId: _parentId, 
            maxDepth: _maxDepth,
            description: _Description);

        Category Action() => result.Value!;

        result.Error.Should().Be(CategoryErrors.ParentNotFound(_parentId));
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        FluentActions.Invoking(Action)
            .Should()
            .Throw<ArgumentException>()
            .Which.Message.Should().Be("invalid success result");
    }
    [Fact]
    public async Task CreateCategoryAsync_WhenParentIdIsNull_Should_Success()
    {
        _mockRepository.Setup(x =>
            x.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        _mockRepository.Verify(x =>
            x.ExistsAsync(_parentId, It.IsAny<CancellationToken>()),
            Times.Never);

        var result = await _sut.CreateCategoryAsync(
            name: _name,
            slug: _slug,
            isActive: _isActive,
            description: _Description);

        result.IsFailure.Should().Be(false);
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(_name);
        result.Value.Slug.Should().Be(_slug);
        result.Value.Path.Should().BeNull();
        result.Value.Description.Should().Be(_Description);
        result.Value.IsActive.Should().Be(_isActive);
        result.Value.Level.Should().Be(0);
    }
    [Fact]
    public async Task CreateCategoryAsync_WithValidParent_Should_SuccessWithCorrectLevel()
    {
        var parentList = new List<Category>
        {
            Category.Create(_name, _slug, 0, _isActive, null, _Description, null),
            Category.Create(_name, _slug, 1, _isActive, Guid.NewGuid(), _Description, null)
        };

        var correctLevel = (short)parentList.Count;

        _mockRepository.Setup(x =>
            x.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        _mockRepository.Setup(x =>
            x.ExistsAsync(_parentId, It.IsAny<CancellationToken>())
            ).ReturnsAsync(true);
        _mockRepository.Setup(x =>
            x.GetAllParentAsync(_parentId, _maxDepth, It.IsAny<CancellationToken>())
            ).ReturnsAsync(parentList);

        var result = await _sut.CreateCategoryAsync(
            name: _name,
            slug: _slug,
            isActive: _isActive,
            parentId: _parentId,
            maxDepth: _maxDepth,
            description: _Description);

        result.IsFailure.Should().Be(false);
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(_name);
        result.Value.Slug.Should().Be(_slug);
        result.Value.Description.Should().Be(_Description);
        result.Value.IsActive.Should().Be(_isActive);
        result.Value.Level.Should().Be(correctLevel);
    }

}
