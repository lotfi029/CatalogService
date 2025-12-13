//using CatalogService.Application.DTOs.Categories;
//using CatalogService.Application.Features.Categories.Commands.UpdateDetails;
//using CatalogService.Domain.Entities;
//using CatalogService.IntegrationTests.Infrastructure;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;

//namespace CatalogService.IntegrationTests.Features.Categories.Commands;

//public class UpdateCategoryDetailsCommandHandlerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTestsCommand<UpdateCategoryDetailsCommand>(factory)
//{
//    [Fact]
//    public async Task HandleAsync_WithValidCommand_Should_UpdateCategoryInDatabase()
//    {
//        var category = Category.Create(
//            name: "Electronics",
//            slug: "electronics",
//            level: 0,
//            parentId: null,
//            isActive: true,
//            description: "Original description");
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var request = new UpdateCategoryDetailsRequest(
//            Name: "Electronics Updated",
//            Description: "Updated description");
//        var command = new UpdateCategoryDetailsCommand(category.Id, request);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsSuccess.Should().BeTrue();

//        var updated = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstOrDefaultAsync(c => c.Id == category.Id);

//        updated.Should().NotBeNull();
//        updated!.Name.Should().Be("Electronics Updated");
//        updated.Description.Should().Be("Updated description");
//        updated.Slug.Should().Be("electronics");
//    }

//    [Fact]
//    public async Task HandleAsync_WithNonExistentCategory_Should_ReturnNotFound()
//    {
//        var request = new UpdateCategoryDetailsRequest("Name", "Description");
//        var command = new UpdateCategoryDetailsCommand(Guid.NewGuid(), request);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();
//        result.Error.Code.Should().Contain("NotFound");
//    }

//    [Fact]
//    public async Task HandleAsync_WithEmptyName_Should_NotUpdateDatabase()
//    {
//        var category = Category.Create(name: "Electronics", slug: "electronics", level: 0, parentId: null, isActive: true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var originalName = category.Name;

//        var request = new UpdateCategoryDetailsRequest("", "Description");
//        var command = new UpdateCategoryDetailsCommand(category.Id, request);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsFailure.Should().BeTrue();

//        var unchanged = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstOrDefaultAsync(c => c.Id == category.Id);

//        unchanged!.Name.Should().Be(originalName);
//    }

//    [Fact]
//    public async Task HandleAsync_WithOnlyNameChange_Should_UpdateOnlyName()
//    {
//        var category = Category.Create(
//            name: "Electronics",
//            slug: "electronics",
//            level: 0,
//            parentId: null,
//            isActive: true,
//            description: "Original description");
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var originalDescription = category.Description;

//        var request = new UpdateCategoryDetailsRequest("Electronics Updated", null);
//        var command = new UpdateCategoryDetailsCommand(category.Id, request);

//        var result = await CommandHandler.HandleAsync(command);

//        result.IsSuccess.Should().BeTrue();

//        var updated = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstOrDefaultAsync(c => c.Id == category.Id);

//        updated!.Name.Should().Be("Electronics Updated");
//        updated.Description.Should().Be(originalDescription);
//    }

//    [Fact]
//    public async Task HandleAsync_WithVeryLongName_Should_RespectDatabaseConstraints()
//    {
//        var category = Category.Create(name: "Electronics", slug: "electronics", level: 0, parentId:null, isActive: true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var longName = new string('A', 300);

//        var request = new UpdateCategoryDetailsRequest(longName, "Description");
//        var command = new UpdateCategoryDetailsCommand(category.Id, request);

//        var act = async () => await CommandHandler.HandleAsync(command);

//        var maxLength = AppDbContext.Model.FindEntityType(typeof(Category))
//            ?.FindProperty("Name")
//            ?.GetMaxLength();

//        if (maxLength.HasValue && maxLength < 300)
//        {
//            var result = await act.Invoke();
//            result.IsFailure.Should().BeTrue();
//        }
//    }

//    [Fact]
//    public async Task HandleAsync_WithMultipleUpdates_Should_KeepLatestValues()
//    {
//        var category = Category.Create(name: "Electronics", slug: "electronics", level: 0, parentId: null, isActive: true);
//        AppDbContext.Categories.Add(category);
//        await AppDbContext.SaveChangesAsync();

//        var request1 = new UpdateCategoryDetailsRequest("Update 1", "Desc 1");
//        await CommandHandler.HandleAsync(new UpdateCategoryDetailsCommand(category.Id, request1));

//        var request2 = new UpdateCategoryDetailsRequest("Update 2", "Desc 2");
//        await CommandHandler.HandleAsync(new UpdateCategoryDetailsCommand(category.Id, request2));

//        var final = await AppDbContext.Categories
//            .AsNoTracking()
//            .FirstAsync(c => c.Id == category.Id);

//        final.Name.Should().Be("Update 2");
//        final.Description.Should().Be("Desc 2");
//    }
//}
