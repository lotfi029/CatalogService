using CatalogService.Application.Features.Categories.Commands.Move;
using CatalogService.Domain.DomainService;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.UnitTests.Features.Categories.Commands;

public class MoveCategoryToNewParentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly Mock<ICategoryDomainService> _mockDomainService;
    private readonly Mock<IDbContextTransaction> _mockTransaction;
    private readonly MoveCategoryToNewParentCommandHandler _sut;
    private readonly Category _sourceCategory;
    private readonly Category _targetParent;

    public MoveCategoryToNewParentCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<ICategoryRepository>();
        _mockDomainService = new Mock<ICategoryDomainService>();
        _mockTransaction = new Mock<IDbContextTransaction>();

        _sut = new MoveCategoryToNewParentCommandHandler(
            _mockUnitOfWork.Object,
            _mockRepository.Object,
            _mockDomainService.Object,
            NullLogger<MoveCategoryToNewParentCommandHandler>.Instance);

        _sourceCategory = Category.Create(
            name: "Laptops", 
            slug: "laptops",
            level: 1, 
            isActive: true,
            parentId: Guid.NewGuid());
        _targetParent = Category.Create(
            name: "Electronics", 
            slug: "electronics",
            level: 0, 
            parentId: null, 
            isActive: true);

        _mockUnitOfWork
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_Should_ReturnSuccess()
    {
        var command = new MoveCategoryToNewParentCommand(_sourceCategory.Id, _targetParent.Id);

        _mockRepository
            .Setup(x => x.FindByIdAsync(_sourceCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sourceCategory);

        _mockRepository
            .Setup(x => x.FindByIdAsync(_targetParent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_targetParent);

        var updatedCategories = new List<Category> { _sourceCategory };

        _mockDomainService
            .Setup(x => x.MoveToNewParent(_sourceCategory.Id, _targetParent, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(updatedCategories));

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();

        _mockRepository.Verify(
            x => x.UpdateRange(updatedCategories),
            Times.Once);

        _mockUnitOfWork.Verify(
            x => x.CommitTransactionAsync(_mockTransaction.Object, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockTransaction.Verify(
            x => x.RollbackAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyCategoryId_Should_ReturnInvalidId()
    {
        var command = new MoveCategoryToNewParentCommand(Guid.Empty, _targetParent.Id);

        var result = await _sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.InvalidId);

        _mockRepository.Verify(
            x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyParentId_Should_ReturnInvalidId()
    {
        var command = new MoveCategoryToNewParentCommand(_sourceCategory.Id, Guid.Empty);

        var result = await _sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.InvalidId);
    }

    [Fact]
    public async Task HandleAsync_WithSameIdAsParent_Should_ReturnCannotMoveToSelf()
    {
        var categoryId = Guid.NewGuid();
        var command = new MoveCategoryToNewParentCommand(categoryId, categoryId);

        var result = await _sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.CannotMoveToSelf);

        _mockRepository.Verify(
            x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentCategory_Should_ReturnNotFound()
    {
        var command = new MoveCategoryToNewParentCommand(Guid.NewGuid(), _targetParent.Id);

        _mockRepository
            .Setup(x => x.FindByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var result = await _sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NotFound(command.Id));

        _mockRepository.Verify(
            x => x.FindByIdAsync(_targetParent.Id, It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentParent_Should_ReturnParentNotFound()
    {
        var command = new MoveCategoryToNewParentCommand(_sourceCategory.Id, Guid.NewGuid());

        _mockRepository
            .Setup(x => x.FindByIdAsync(_sourceCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sourceCategory);

        _mockRepository
            .Setup(x => x.FindByIdAsync(command.NewParentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var result = await _sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.ParentNotFound(command.NewParentId));
    }

    [Fact]
    public async Task HandleAsync_WithSameParent_Should_ReturnAlreadyHasThisParent()
    {
        var parentId = Guid.NewGuid();

        var category = Category.Create(
            name: "Laptops", 
            slug: "laptops", 
            level: 1, 
            parentId: parentId, 
            isActive: true);

        var command = new MoveCategoryToNewParentCommand(category.Id, parentId);

        _mockRepository
            .Setup(x => x.FindByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var result = await _sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.AlreadyHasThisParent);

        _mockRepository.Verify(
            x => x.FindByIdAsync(parentId, It.IsAny<CancellationToken>()),
            Times.Never);
    }
    [Fact]
    public async Task HandleAsync_WhenExceptionOccurs_Should_RollbackAndReturnUnexpectedError()
    {
        var command = new MoveCategoryToNewParentCommand(_sourceCategory.Id, _targetParent.Id);

        _mockRepository
            .Setup(x => x.FindByIdAsync(_sourceCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sourceCategory);

        _mockRepository
            .Setup(x => x.FindByIdAsync(_targetParent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_targetParent);

        _mockDomainService
            .Setup(x => x.MoveToNewParent(_sourceCategory.Id, _targetParent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var result = await _sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Error.Unexpected");

        _mockTransaction.Verify(
            x => x.RollbackAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(
            x => x.CommitTransactionAsync(It.IsAny<IDbContextTransaction>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WithCancellationToken_Should_PassThroughAllLayers()
    {
        var command = new MoveCategoryToNewParentCommand(_sourceCategory.Id, _targetParent.Id);
        var cts = new CancellationTokenSource();
        var ct = cts.Token;

        _mockRepository
            .Setup(x => x.FindByIdAsync(_sourceCategory.Id, ct))
            .ReturnsAsync(_sourceCategory);

        _mockRepository
            .Setup(x => x.FindByIdAsync(_targetParent.Id, ct))
            .ReturnsAsync(_targetParent);

        var updatedCategories = new List<Category> { _sourceCategory };
        _mockDomainService
            .Setup(x => x.MoveToNewParent(_sourceCategory.Id, _targetParent, ct))
            .ReturnsAsync(Result.Success(updatedCategories));

        _mockUnitOfWork
            .Setup(x => x.BeginTransactionAsync(ct))
            .ReturnsAsync(_mockTransaction.Object);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(ct))
            .ReturnsAsync(1);

        var result = await _sut.HandleAsync(command, ct);

        result.IsSuccess.Should().BeTrue();

        _mockDomainService.Verify(
            x => x.MoveToNewParent(_sourceCategory.Id, _targetParent, ct),
            Times.Once);

        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(ct),
            Times.Once);
    }
}