//using CatalogService.Application.Features.Categories.Commands.Move;
//using CatalogService.Domain.Entities;
//using CatalogService.IntegrationTests.Infrastructure;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;

//namespace CatalogService.IntegrationTests.Features.Categories.Commands;

//public class MoveCategoryToNewParentCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTestsCommand<MoveCategoryToNewParentCommand>(factory)
//{
//    [Fact]
//    public async Task HandleAsync_WithValidCommand_Should_MoveCategoryToNewParent()
//    {
//        var root = Category.Create(
//            name: "Electronics",
//            slug: "electronics",
//            level: 0,
//            parentId: null,
//            isActive: true) ;


//        var oldParent = Category.Create(
//            name: "Old Parent",
//            slug: "old-parent",
//            level: 1,
//            parentId: root.Id,
//            isActive: true);

//        var newParent = Category.Create(
//            name: "New Parent",
//            slug: "new-parent",
//            level: 1,
//            parentId: root.Id,
//            isActive: true);

//        var categoryToMove = Category.Create(
//            name: "Product",
//            slug: "product",
//            level: 2,
//            parentId: oldParent.Id,
//            isActive: true);

//        AppDbContext.Categories.AddRange(root, oldParent, newParent, categoryToMove);
//        await AppDbContext.SaveChangesAsync();

//        var command = new MoveCategoryToNewParentCommand(categoryToMove.Id, newParent.Id);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsSuccess.Should().BeTrue();

//        var movedCategory = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstAsync(c => c.Id == categoryToMove.Id);

//        movedCategory.ParentId.Should().Be(newParent.Id);
//        movedCategory.Level.Should().Be(2);
//    }

//    [Fact]
//    public async Task HandleAsync_WithNonExistentCategory_Should_ReturnNotFound()
//    {
//        var parent = Category.Create(
//            name: "Parent",
//            slug: "parent",
//            level: 0,
//            parentId: null,
//            isActive: true);

//        AppDbContext.Categories.Add(parent);
//        await AppDbContext.SaveChangesAsync();

//        var command = new MoveCategoryToNewParentCommand(Guid.NewGuid(), parent.Id);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.Error.Code.Should().Contain("NotFound");
//    }

//    [Fact]
//    public async Task HandleAsync_WithNonExistentParent_Should_ReturnParentNotFound()
//    {
//        var category = Category.Create(
//            name: "Category",
//            slug: "category",
//            level: 0,
//            parentId: null,
//            isActive: true);

//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var command = new MoveCategoryToNewParentCommand(category.Id, Guid.NewGuid());

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.Error.Code.Should().Contain("Parent");
//    }

//    [Fact]
//    public async Task HandleAsync_WithSameIdAsParent_Should_ReturnCannotMoveToSelf()
//    {
//        var category = Category.Create(
//            name: "Category",
//            slug: "category",
//            level: 0,
//            parentId: null,
//            isActive: true);

//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var command = new MoveCategoryToNewParentCommand(category.Id, category.Id);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.Error.Code.Should().Contain("Self");
//    }

//    [Fact]
//    public async Task HandleAsync_WithDeepHierarchy_Should_UpdateAllDescendantLevels()
//    {
//        var root = Category.Create(
//            name: "Root",
//            slug: "root",
//            level: 0,
//            parentId: null,
//            isActive: true);

//        var branch1 = Category.Create(
//            name: "Branch1",
//            slug: "branch1",
//            level: 1,
//            parentId: root.Id,
//            isActive: true);

//        var leaf = Category.Create(
//            name: "Leaf",
//            slug: "leaf",
//            level: 2,
//            parentId: branch1.Id,
//            isActive: true);

//        var newBranch = Category.Create(
//            name: "NewBranch",
//            slug: "new-branch",
//            level: 1,
//            parentId: root.Id,
//            isActive: true);

//        AppDbContext.Categories.AddRange(root, branch1, leaf, newBranch);
//        await AppDbContext.SaveChangesAsync();

//        var command = new MoveCategoryToNewParentCommand(branch1.Id, newBranch.Id);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsSuccess.Should().BeTrue();

//        var updatedBranch = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstAsync(c => c.Id == branch1.Id);

//        var updatedLeaf = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstAsync(c => c.Id == leaf.Id);

//        updatedBranch.ParentId.Should().Be(newBranch.Id);
//        updatedBranch.Level.Should().Be(2);
//        updatedLeaf.Level.Should().Be(3);
//    }

//    [Fact]
//    public async Task HandleAsync_WithSameParent_Should_ReturnFailure()
//    {
//        var parent = Category.Create(
//            name: "Parent",
//            slug: "parent",
//            level: 0,
//            parentId: null,
//            isActive: true);

//        var child = Category.Create(
//            name: "Child",
//            slug: "child",
//            level: 1,
//            parentId: parent.Id,
//            isActive: true);

//        AppDbContext.Categories.AddRange(parent, child);
//        await AppDbContext.SaveChangesAsync();

//        var command = new MoveCategoryToNewParentCommand(child.Id, parent.Id);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//    }
//}