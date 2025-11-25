using CatalogService.Application.Features.Categories.Commands.Create;
using CatalogService.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.IntegrationTests.Features.Categories.Commands;

public class CreateCategoryCommandHandlerTests(IntegrationTestWebAppFactory factory) 
    : BaseIntegrationTestsCommand<CreateCategoryCommand, Guid>(factory)
{
    [Fact]
    
    public async Task HandleAsync_WithValidCommand_Should_CreateCategoryInDatabase()
    {
        var command = new CreateCategoryCommand(
            Name: "Electronics",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: "Electronic products");

        var result = await CommandHandler.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var savedCategory = await AppDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == result.Value);

        savedCategory.Should().NotBeNull();
        savedCategory!.Name.Should().Be("Electronics");
        savedCategory.Slug.Should().Be("electronics");
        savedCategory.IsActive.Should().BeTrue();
        savedCategory.ParentId.Should().BeNull();
        savedCategory.Level.Should().Be(0);
        savedCategory.Description.Should().Be("Electronic products");
    }

    [Fact]
    public async Task HandleAsync_WithDuplicateSlug_Should_ReturnFailure()
    {
        var firstCommand = new CreateCategoryCommand(
            Name: "Electronics",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: null);

        await CommandHandler.HandleAsync(firstCommand);

        var duplicateCommand = new CreateCategoryCommand(
            Name: "Electronics V2",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: null);

        var result = await CommandHandler.HandleAsync(duplicateCommand);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("Slug");

        var categoryCount = await AppDbContext.Categories
            .CountAsync(c => c.Slug == "electronics");
        categoryCount.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_WithValidParent_Should_CreateChildCategoryWithCorrectLevel()
    {
        var parentCommand = new CreateCategoryCommand(
            Name: "Electronics",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: null);

        var parentResult = await CommandHandler.HandleAsync(parentCommand);

        var childCommand = new CreateCategoryCommand(
            Name: "Laptops",
            Slug: "laptops",
            IsActive: true,
            ParentId: parentResult.Value,
            Description: "Laptop computers");

        var childResult = await CommandHandler.HandleAsync(childCommand);

        childResult.IsSuccess.Should().BeTrue();

        var childCategory = await AppDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == childResult.Value);

        childCategory.Should().NotBeNull();
        childCategory!.Name.Should().Be("Laptops");
        childCategory.ParentId.Should().Be(parentResult.Value);
        childCategory.Level.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentParent_Should_ReturnFailure()
    {
        var nonExistentParentId = Guid.NewGuid();

        var command = new CreateCategoryCommand(
            Name: "Laptops",
            Slug: "laptops",
            IsActive: true,
            ParentId: nonExistentParentId,
            Description: null);

        var result = await CommandHandler.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("Parent");

        var categoryCount = await AppDbContext.Categories.CountAsync();
        categoryCount.Should().Be(0);
    }

    [Fact]
    public async Task HandleAsync_WithDeepHierarchy_Should_CalculateCorrectLevel()
    {
        var rootCommand = new CreateCategoryCommand(
            Name: "Electronics",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: null);
        var rootResult = await CommandHandler.HandleAsync(rootCommand);

        var level1Command = new CreateCategoryCommand(
            Name: "Computers",
            Slug: "computers",
            IsActive: true,
            ParentId: rootResult.Value,
            Description: null);
        var level1Result = await CommandHandler.HandleAsync(level1Command);

        var level2Command = new CreateCategoryCommand(
            Name: "Laptops",
            Slug: "laptops",
            IsActive: true,
            ParentId: level1Result.Value,
            Description: null);
        var level2Result = await CommandHandler.HandleAsync(level2Command);

        var level3Command = new CreateCategoryCommand(
            Name: "Gaming Laptops",
            Slug: "gaming-laptops",
            IsActive: true,
            ParentId: level2Result.Value,
            Description: null);
        var level3Result = await CommandHandler.HandleAsync(level3Command);

        level3Result.IsSuccess.Should().BeTrue();

        var gamingLaptops = await AppDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == level3Result.Value);

        gamingLaptops.Should().NotBeNull();
        gamingLaptops!.Level.Should().Be(3);
        gamingLaptops.ParentId.Should().Be(level2Result.Value);
    }

    [Fact]
    public async Task HandleAsync_WithInactiveCategory_Should_PersistIsActiveFlag()
    {
        var command = new CreateCategoryCommand(
            Name: "Deprecated",
            Slug: "deprecated",
            IsActive: false,
            ParentId: null,
            Description: "Deprecated category");

        var result = await CommandHandler.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();

        var category = await AppDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == result.Value);

        category.Should().NotBeNull();
        category!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task HandleAsync_Should_SetAuditFields()
    {
        var command = new CreateCategoryCommand(
            Name: "Electronics",
            Slug: "electronics",
            IsActive: true,
            ParentId: null,
            Description: null);

        var result = await CommandHandler.HandleAsync(command);

        var category = await AppDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == result.Value);

        category.Should().NotBeNull();
        category!.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task HandleAsync_MultipleCategories_Should_MaintainUniqueConstraints()
    {
        var commands = new[]
        {
            new CreateCategoryCommand("Electronics", "electronics", true, null, null),
            new CreateCategoryCommand("Clothing", "clothing", true, null, null),
            new CreateCategoryCommand("Books", "books", true, null, null)
        };

        foreach (var command in commands)
        {
            var result = await CommandHandler.HandleAsync(command);
            result.IsSuccess.Should().BeTrue();
        }

        var categoryCount = await AppDbContext.Categories.CountAsync();
        categoryCount.Should().Be(3);

        var slugs = await AppDbContext.Categories
            .Select(c => c.Slug)
            .ToListAsync();

        slugs.Should().OnlyHaveUniqueItems();
    }
}
