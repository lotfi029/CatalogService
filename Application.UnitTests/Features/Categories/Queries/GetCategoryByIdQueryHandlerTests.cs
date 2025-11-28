using CatalogService.Application;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Queries.GetById;
using Dapper;
using Moq.Dapper;
using System.Data;

namespace Application.UnitTests.Features.Categories.Queries;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<IDbConnectionFactory> _mockFactory;
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly GetCategoryByIdQueryHandler _sut;

    public GetCategoryByIdQueryHandlerTests()
    {
        _mockFactory = new Mock<IDbConnectionFactory>();
        _mockConnection = new Mock<IDbConnection>();

        _mockFactory
            .Setup(f => f.CreateConnection())
            .Returns(_mockConnection.Object);

        _sut = new GetCategoryByIdQueryHandler(
            _mockFactory.Object,
            NullLogger<GetCategoryByIdQueryHandler>.Instance);
    }

    [Fact]
    public async Task HandleAsync_WithValidId_Should_ReturnCategory()
    {
        var categoryId = Guid.NewGuid();
        var expectedCategory = new CategoryDetailedResponse(
            Id: categoryId,
            Name: "Electronics",
            Slug: "electronics",
            ParentId: null,
            Level: 0,
            Description: "Electronic products",
            Path: "electronics");

        _mockConnection
            .SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<CategoryDetailedResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
            .ReturnsAsync(expectedCategory);

        var query = new GetCategoryByIdQuery(categoryId);

        var result = await _sut.HandleAsync(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(categoryId);
        result.Value.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentId_Should_ReturnNotFound()
    {
        var categoryId = Guid.NewGuid();

        _mockConnection
            .SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<CategoryDetailedResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
            .ReturnsAsync((CategoryDetailedResponse?)null);

        var query = new GetCategoryByIdQuery(categoryId);

        var result = await _sut.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NotFound(categoryId));
    }

    [Fact]
    public async Task HandleAsync_WithEmptyGuid_Should_ReturnInvalidId()
    {
        var query = new GetCategoryByIdQuery(Guid.Empty);

        var result = await _sut.HandleAsync(query);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.InvalidId);
    }

    [Fact]
    public async Task HandleAsync_Should_CreateConnection()
    {
        var categoryId = Guid.NewGuid();

        _mockConnection
            .SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<CategoryDetailedResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
            .ReturnsAsync(new CategoryDetailedResponse(categoryId, "Test", "test", null, 0, null, null));

        var query = new GetCategoryByIdQuery(categoryId);

        await _sut.HandleAsync(query);

        _mockFactory.Verify(f => f.CreateConnection(), Times.Once);
    }
}
