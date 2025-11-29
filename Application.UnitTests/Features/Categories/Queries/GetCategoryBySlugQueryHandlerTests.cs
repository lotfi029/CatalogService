using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.Categories.Queries.GetBySlug;

namespace Application.UnitTests.Features.Categories.Queries;

public class GetCategoryBySlugQueryHandlerTests
{
    private readonly Mock<ICategoryQueries> _mockQueries;
    private readonly GetCategoryBySlugQueryHandler _sut;

    public GetCategoryBySlugQueryHandlerTests()
    {
        _mockQueries = new Mock<ICategoryQueries>();

        _sut = new GetCategoryBySlugQueryHandler(
            NullLogger<GetCategoryBySlugQueryHandler>.Instance,
            _mockQueries.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidSlug_Should_ReturnCategory()
    {
        var slug = "electronics";
        var expectedCategory = new CategoryDetailedResponse(
            Guid.NewGuid(),
            "Electronics",
            slug,
            null,
            1,
            "Electronics and gadgets",
            "electronics");

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedCategory));

        var query = new GetCategoryBySlugQuery(slug);

        var result = await _sut.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Slug.Should().Be(slug);
        result.Value.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentSlug_Should_ReturnSlugNotFound()
    {
        var slug = "non-existent";
        var error = CategoryErrors.SlugNotFound(slug);

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        var query = new GetCategoryBySlugQuery(slug);

        var result = await _sut.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task HandleAsync_WithInvalidSlug_Should_ReturnInvalidSlug(string? invalidSlug)
    {
        var query = new GetCategoryBySlugQuery(invalidSlug!);

        var result = await _sut.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.InvalidSlug);

        _mockQueries.Verify(
            x => x.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithException_Should_ReturnUnexpectedError()
    {
        var slug = "error-slug";
        var expectedException = new InvalidOperationException("Database error");

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var query = new GetCategoryBySlugQuery(slug);

        var result = await _sut.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Error.Unexpected");
        result.Error.Description.Should().Be("Error Occurred while retrive category by slug");
    }

    [Fact]
    public async Task HandleAsync_Should_PassCancellationToken()
    {
        var slug = "electronics";
        var cts = new CancellationTokenSource();
        var category = new CategoryDetailedResponse(
            Guid.NewGuid(), "Test", slug, null, 1, null, "test");

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, cts.Token))
            .ReturnsAsync(Result.Success(category));

        var query = new GetCategoryBySlugQuery(slug);

        await _sut.HandleAsync(query, cts.Token);

        _mockQueries.Verify(
            x => x.GetBySlugAsync(slug, cts.Token),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithCategoryWithParent_Should_ReturnParentId()
    {
        var slug = "laptops";
        var parentId = Guid.NewGuid();
        var category = new CategoryDetailedResponse(
            Guid.NewGuid(),
            "Laptops",
            slug,
            parentId,
            2,
            "Laptop computers",
            "electronics/laptops");

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(category));

        var query = new GetCategoryBySlugQuery(slug);

        var result = await _sut.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().Be(parentId);
        result.Value.Level.Should().Be(2);
    }

    [Fact]
    public async Task HandleAsync_WithRootCategory_Should_ReturnNullParentId()
    {
        var slug = "electronics";
        var category = new CategoryDetailedResponse(
            Guid.NewGuid(),
            "Electronics",
            slug,
            null,
            1,
            "Electronic products",
            "electronics");

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(category));

        var query = new GetCategoryBySlugQuery(slug);

        var result = await _sut.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ParentId.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_WithNullDescription_Should_ReturnNullDescription()
    {
        var slug = "test";
        var category = new CategoryDetailedResponse(
            Guid.NewGuid(),
            "Test",
            slug,
            null,
            1,
            null,
            "test");

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(category));

        var query = new GetCategoryBySlugQuery(slug);

        var result = await _sut.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("electronics")]
    [InlineData("books-fiction")]
    [InlineData("home-garden")]
    public async Task HandleAsync_WithDifferentSlugs_Should_QueryCorrectly(string slug)
    {
        var category = new CategoryDetailedResponse(
            Guid.NewGuid(), "Category", slug, null, 1, null, slug);

        _mockQueries
            .Setup(x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(category));

        var query = new GetCategoryBySlugQuery(slug);

        var result = await _sut.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Slug.Should().Be(slug);

        _mockQueries.Verify(
            x => x.GetBySlugAsync(slug, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}