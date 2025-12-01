using CatalogService.Domain.DomainService.Categories;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Errors;
using CatalogService.Domain.IRepositories;
using FluentAssertions;
using Moq;
using SharedKernel;
using System.Linq.Expressions;

namespace Domain.UnitTests.DomainService;

public sealed class CreateCategoryDomainServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly Mock<ICategoryVariantAttributeRepository> _mokeCategoryVariantRepository;
    private readonly CategoryDomainService _sut;
    const string _name = "Electronics";
    const string _slug = "electronics";
    const string _Description = "Electronics Products";
    const bool _isActive = true;
    readonly Guid _parentId = Guid.CreateVersion7();
    public CreateCategoryDomainServiceTests()
    {
        _mockRepository = new Mock<ICategoryRepository>();
        _mokeCategoryVariantRepository = new Mock<ICategoryVariantAttributeRepository>();
        _sut = new CategoryDomainService(_mockRepository.Object, _mokeCategoryVariantRepository.Object);
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
        result.Value.Path.Should().Be(_slug);
        result.Value.Description.Should().Be(_Description);
        result.Value.IsActive.Should().Be(_isActive);
        result.Value.Level.Should().Be(0);
    }
    [Fact]
    public async Task CreateCategoryAsync_WithValidParent_Should_SuccessWithCorrectLevel()
    {
        var parent = Category.Create(_name, _slug, 1, _isActive, Guid.NewGuid(), _Description, null);
        var correctLevel = (short)(parent.Level + 1);
        _mockRepository.Setup(x =>
            x.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        _mockRepository.Setup(x =>
            x.ExistsAsync(_parentId, It.IsAny<CancellationToken>())
            ).ReturnsAsync(true);
        _mockRepository.Setup(x =>
            x.FindByIdAsync(_parentId, It.IsAny<CancellationToken>())
            ).ReturnsAsync(parent);

        var result = await _sut.CreateCategoryAsync(
            name: _name,
            slug: _slug,
            isActive: _isActive,
            parentId: _parentId,
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
