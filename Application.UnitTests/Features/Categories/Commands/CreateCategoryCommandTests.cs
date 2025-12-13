using CatalogService.Application.Features.Categories.Commands.Create;
using CatalogService.Domain.DomainService.Categories;

namespace Application.UnitTests.Features.Categories.Commands;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICategoryDomainService> _mockDomainService;
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly CreateCategoryCommandHandler _sut;
    private readonly Category _testCat;

    public CreateCategoryCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDomainService = new Mock<ICategoryDomainService>();
        _mockRepository = new Mock<ICategoryRepository>();

        _sut = new CreateCategoryCommandHandler(
            _mockUnitOfWork.Object,
            _mockDomainService.Object,
            _mockRepository.Object,
            NullLogger<CreateCategoryCommandHandler>.Instance);

        _testCat = Category.Create(
            name: "Electronics",
            slug: "electronics",
            level: 0,
            isActive: true,
            description: "Electronic products").Value!;
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnSuccessWithCategoryId_WhenCommandIsValid()
    {
        var command = new CreateCategoryCommand(
            Name: _testCat.Name,
            Slug: _testCat.Slug,
            IsActive: _testCat.IsActive,
            ParentId: _testCat.ParentId,
            Description: _testCat.Description);

        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                command.IsActive,
                command.ParentId,
                command.Description,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(_testCat));

        _mockRepository
            .Setup(x => x.Add(_testCat))
            .Returns(_testCat.Id);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.HandleAsync(command, It.IsAny<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(_testCat.Id);
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);

        _mockDomainService.Verify(
            x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                command.IsActive,
                command.ParentId,
                command.Description,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mockRepository.Verify(
            x => x.Add(_testCat),
            Times.Once);

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnFailureWithoutSaving_WhenDomainServiceReturnsFailure()
    {
        var command = new CreateCategoryCommand(
            Name: "Electronics",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: null);

        var error = CategoryErrors.SlugAlreadyExist("electronics");
        
        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<Category>(error));

        var result = await _sut.HandleAsync(command, It.IsAny<CancellationToken>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
        result.IsSuccess.Should().BeFalse();

        _mockRepository.Verify(
            x => x.Add(It.IsAny<Category>()),
            Times.Never);

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnUnexpectedError_WhenDomainServiceThrowsException()
    {
        var command = new CreateCategoryCommand(
            Name: "Electronics",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: null);

        var exception = new InvalidOperationException("Database error");

        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var result = await _sut.HandleAsync(command, It.IsAny<CancellationToken>());

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Error.Unexpected");
        result.Error.Description.Should().Be("Error occurred while creating category");

        _mockRepository.Verify(
            x => x.Add(It.IsAny<Category>()),
            Times.Never);

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnUnexpectedError_WhenRepositoryThrowsException()
    {
        var command = new CreateCategoryCommand(
            Name: _testCat.Name,
            Slug: _testCat.Slug,
            IsActive: _testCat.IsActive,
            ParentId: _testCat.ParentId,
            Description: _testCat.Description);

        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(_testCat));

        _mockRepository
            .Setup(x => x.Add(It.IsAny<Category>()))
            .Throws(new InvalidOperationException("Repository error"));

        var result = await _sut.HandleAsync(command, It.IsAny<CancellationToken>());

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Error.Unexpected");

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnUnexpectedError_WhenUnitOfWorkThrowsException()
    {
        var command = new CreateCategoryCommand(
            Name: _testCat.Name,
            Slug: _testCat.Slug,
            IsActive: _testCat.IsActive,
            ParentId: _testCat.ParentId,
            Description: _testCat.Description);

        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(_testCat));

        _mockRepository
            .Setup(x => x.Add(It.IsAny<Category>()))
            .Returns(_testCat.Id);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("SaveChanges failed"));

        var result = await _sut.HandleAsync(command, It.IsAny<CancellationToken>());

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Error.Unexpected");
    }

    [Fact]
    public async Task HandleAsync_Should_PassThroughAllLayers_WithCancellationToken()
    {
        var command = new CreateCategoryCommand(
            Name: _testCat.Name,
            Slug: _testCat.Slug,
            IsActive: _testCat.IsActive,
            ParentId: _testCat.ParentId,
            Description: _testCat.Description);

        var cts = new CancellationTokenSource();
        var ct = cts.Token;

        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                command.IsActive,
                command.ParentId,
                command.Description,
                ct))
            .ReturnsAsync(Result.Success(_testCat));

        _mockRepository
            .Setup(x => x.Add(_testCat))
            .Returns(_testCat.Id);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(ct))
            .ReturnsAsync(1);

        var result = await _sut.HandleAsync(command, ct);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(_testCat.Id);

        _mockDomainService.Verify(
            x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                command.IsActive,
                command.ParentId,
                command.Description,
                ct),
            Times.Once);

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(ct),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnSuccessWithCategoryId_WithChildCategory()
    {
        var parentId = Guid.NewGuid();
        var childCat = Category.Create(
            name: "Laptops",
            slug: "laptops",
            level: 1,
            parentId: parentId,
            isActive: true,
            description: "Laptop computers").Value!;

        var command = new CreateCategoryCommand(
            Name: childCat.Name,
            Slug: childCat.Slug,
            IsActive: childCat.IsActive,
            ParentId: parentId,
            Description: childCat.Description);

        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                command.IsActive,
                command.ParentId,
                command.Description,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(childCat));

        _mockRepository
            .Setup(x => x.Add(childCat))
            .Returns(childCat.Id);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.HandleAsync(command, It.IsAny<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(childCat.Id);
        result.IsFailure.Should().BeFalse();

        _mockDomainService.Verify(
            x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                command.IsActive,
                parentId,
                command.Description,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mockRepository.Verify(
            x => x.Add(childCat),
            Times.Once);

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnSuccessWithCategoryId_WithInactiveCategory()
    {
        var inactiveCat = Category.Create(
            name: "Deprecated",
            slug: "deprecated",
            level: 0,
            isActive: false,
            description: "Deprecated category").Value!;

        var command = new CreateCategoryCommand(
            Name: inactiveCat.Name,
            Slug: inactiveCat.Slug,
            IsActive: false,
            ParentId: null,
            Description: inactiveCat.Description);

        _mockDomainService
            .Setup(x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                false,
                command.ParentId,
                command.Description,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(inactiveCat));

        _mockRepository
            .Setup(x => x.Add(inactiveCat))
            .Returns(inactiveCat.Id);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.HandleAsync(command, It.IsAny<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(inactiveCat.Id);

        _mockDomainService.Verify(
            x => x.CreateCategoryAsync(
                command.Name,
                command.Slug,
                false,
                command.ParentId,
                command.Description,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}