//using CatalogService.Application.DTOs.Categories;
//using CatalogService.Application.Features.Categories.Commands.UpdateDetails;

//namespace Application.UnitTests.Features.Categories.Commands;

//public class UpdateCategoryDetailsCommandHandlerTests
//{
//    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
//    private readonly Mock<ICategoryRepository> _mockRepository;
//    private readonly UpdateCategoryDetailsCommandHandler _sut;
//    private readonly Category _testCategory;

//    public UpdateCategoryDetailsCommandHandlerTests()
//    {
//        _mockUnitOfWork = new Mock<IUnitOfWork>();
//        _mockRepository = new Mock<ICategoryRepository>();

//        _sut = new UpdateCategoryDetailsCommandHandler(
//            _mockUnitOfWork.Object,
//            _mockRepository.Object,
//            NullLogger<UpdateCategoryDetailsCommandHandler>.Instance);

//        _testCategory = Category.Create(
//            name: "Electronics",
//            slug: "electronics",
//            level: 0,
//            isActive: true);
//    }

//    [Fact]
//    public async Task HandleAsync_WithValidCommand_Should_ReturnSuccess()
//    {
//        var request = new UpdateCategoryDetailsRequest(
//            Name: "Electronics Updated",
//            Description: "Updated description");

//        var command = new UpdateCategoryDetailsCommand(_testCategory.Id, request);

//        _mockRepository
//            .Setup(x => x.FindAsync(_testCategory.Id, null, It.IsAny<CancellationToken>()))
//            .ReturnsAsync(_testCategory);

//        _mockUnitOfWork
//            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
//            .ReturnsAsync(1);

//        var result = await _sut.HandleAsync(command);

//        result.IsSuccess.Should().BeTrue();
//        result.IsFailure.Should().BeFalse();

//        _mockRepository.Verify(
//            x => x.FindAsync(_testCategory.Id, null, It.IsAny<CancellationToken>()),
//            Times.Once);

//        _mockRepository.Verify(
//            x => x.Update(It.Is<Category>(c =>
//                c.Id == _testCategory.Id &&
//                c.Name == "Electronics Updated")),
//            Times.Once);

//        _mockUnitOfWork.Verify(
//            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
//            Times.Once);
//    }

//    [Fact]
//    public async Task HandleAsync_WithEmptyId_Should_ReturnFailure()
//    {
//        var request = new UpdateCategoryDetailsRequest("Name", "Description");
//        var command = new UpdateCategoryDetailsCommand(Guid.Empty, request);

//        var result = await _sut.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.IsSuccess.Should().BeFalse();
//        result.Error.Should().Be(CategoryErrors.InvalidId);

//        _mockRepository.Verify(
//            x => x.FindAsync(It.IsAny<Guid>(), null, It.IsAny<CancellationToken>()),
//            Times.Never);
//    }

//    [Fact]
//    public async Task HandleAsync_WithNonExistentCategory_Should_ReturnNotFound()
//    {
//        var categoryId = Guid.NewGuid();
//        var request = new UpdateCategoryDetailsRequest("Name", "Description");
//        var command = new UpdateCategoryDetailsCommand(categoryId, request);

//        _mockRepository
//            .Setup(x => x.FindAsync(categoryId, null, It.IsAny<CancellationToken>()))
//            .ReturnsAsync((Category?)null);

//        var result = await _sut.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.IsSuccess.Should().BeFalse();
//        result.Error.Should().Be(CategoryErrors.NotFound(categoryId));

//        _mockRepository.Verify(
//            x => x.Update(It.IsAny<Category>()),
//            Times.Never);

//        _mockUnitOfWork.Verify(
//            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
//            Times.Never);
//    }

//    [Fact]
//    public async Task HandleAsync_WhenRepositoryThrowsException_Should_ReturnUnexpectedError()
//    {
//        var request = new UpdateCategoryDetailsRequest("Updated", "Description");
//        var command = new UpdateCategoryDetailsCommand(_testCategory.Id, request);

//        _mockRepository
//            .Setup(x => x.FindAsync(_testCategory.Id, null, It.IsAny<CancellationToken>()))
//            .ReturnsAsync(_testCategory);

//        _mockRepository
//            .Setup(x => x.Update(It.IsAny<Category>()))
//            .Throws(new InvalidOperationException("Database error"));

//        var result = await _sut.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.Error.Code.Should().Be("Error.Unexpected");

//        _mockUnitOfWork.Verify(
//            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
//            Times.Never);
//    }

//    [Fact]
//    public async Task HandleAsync_WhenUnitOfWorkThrowsException_Should_ReturnUnexpectedError()
//    {
//        var request = new UpdateCategoryDetailsRequest("Updated", "Description");
//        var command = new UpdateCategoryDetailsCommand(_testCategory.Id, request);

//        _mockRepository
//            .Setup(x => x.FindAsync(_testCategory.Id, null, It.IsAny<CancellationToken>()))
//            .ReturnsAsync(_testCategory);

//        _mockUnitOfWork
//            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
//            .ThrowsAsync(new InvalidOperationException("SaveChanges failed"));

//        var result = await _sut.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.Error.Code.Should().Be("Error.Unexpected");
//    }

//    [Fact]
//    public async Task HandleAsync_WithNullDescription_Should_UpdateOnlyName()
//    {
//        var request = new UpdateCategoryDetailsRequest("Updated Name", null);
//        var command = new UpdateCategoryDetailsCommand(_testCategory.Id, request);

//        _mockRepository
//            .Setup(x => x.FindAsync(_testCategory.Id, null,It.IsAny<CancellationToken>()))
//            .ReturnsAsync(_testCategory);

//        _mockUnitOfWork
//            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
//            .ReturnsAsync(1);

//        var result = await _sut.HandleAsync(command);

//        result.IsSuccess.Should().BeTrue();

//        _mockRepository.Verify(
//            x => x.Update(It.Is<Category>(c => c.Name == "Updated Name")),
//            Times.Once);
//    }

//    [Fact]
//    public async Task HandleAsync_WithCancellationToken_Should_PassThroughAllLayers()
//    {
//        var request = new UpdateCategoryDetailsRequest("Updated", "Description");
//        var command = new UpdateCategoryDetailsCommand(_testCategory.Id, request);
//        var cts = new CancellationTokenSource();
//        var ct = cts.Token;

//        _mockRepository
//            .Setup(x => x.FindAsync(_testCategory.Id,null, ct))
//            .ReturnsAsync(_testCategory);

//        _mockUnitOfWork
//            .Setup(x => x.SaveChangesAsync(ct))
//            .ReturnsAsync(1);

//        var result = await _sut.HandleAsync(command, ct);

//        result.IsSuccess.Should().BeTrue();

//        _mockRepository.Verify(
//            x => x.FindAsync(_testCategory.Id, null, ct),
//            Times.Once);

//        _mockUnitOfWork.Verify(
//            x => x.SaveChangesAsync(ct),
//            Times.Once);
//    }

//    [Fact]
//    public async Task HandleAsync_WithSameValues_Should_StillUpdateAndSave()
//    {
//        var request = new UpdateCategoryDetailsRequest(
//            _testCategory.Name,
//            _testCategory.Description);
//        var command = new UpdateCategoryDetailsCommand(_testCategory.Id, request);

//        _mockRepository
//            .Setup(x => x.FindAsync(_testCategory.Id, null, It.IsAny<CancellationToken>()))
//            .ReturnsAsync(_testCategory);

//        _mockUnitOfWork
//            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
//            .ReturnsAsync(1);

//        var result = await _sut.HandleAsync(command);

//        result.IsSuccess.Should().BeTrue();

//        _mockRepository.Verify(
//            x => x.Update(It.IsAny<Category>()),
//            Times.Once);

//        _mockUnitOfWork.Verify(
//            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
//            Times.Once);
//    }
//}