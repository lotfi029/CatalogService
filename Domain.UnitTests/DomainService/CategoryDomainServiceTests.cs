using CatalogService.Domain.DomainService;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Errors;
using CatalogService.Domain.IRepositories;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Domain.UnitTests.DomainService;

public sealed class CategoryDomainServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly CategoryDomainService _sut;
    const string _name = "Electronics";
    const string _slug = "electronics";
    const string _Description = "Electronics Products";
    Guid _parentId = Guid.CreateVersion7();
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
            _name,
            _slug);

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
            _name, 
            _slug, 
            _parentId, 
            100);

        Category Action() => result.Value!;

        result.Error.Should().Be(CategoryErrors.ParentNotFound(_parentId));
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        FluentActions.Invoking(Action)
            .Should()
            .Throw<ArgumentException>()
            .Which.Message.Should().Be("invalid success result");
    }

}
