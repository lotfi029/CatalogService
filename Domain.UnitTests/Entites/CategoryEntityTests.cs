//using CatalogService.Domain.DomainEvents.Categories;
//using CatalogService.Domain.Entities;
//using FluentAssertions;
//using System.Security.AccessControl;

//namespace Domain.UnitTests.Entites;

//public sealed class CategoryEntityTests
//{
//    [Fact]
//    public void Create_WithValidData_Should_ReturnCategory()
//    {
//        var name = "Electronics";
//        var slug = "electronics";
//        var level = (short)0;
//        var isActive = true;

//        var category = Category.Create(name, slug, level, isActive).Value;

//        category.Should().NotBeNull();
//        category.Name.Should().Be(name);
//        category.Slug.Should().Be(slug);
//        category.Level.Should().Be(level);
//        category.IsActive.Should().BeTrue();
//        category.ParentId.Should().BeNull();
//        category.Path.Should().Be(slug);
//    }

//    [Fact]
//    public void Create_WithParent_Should_CreatePathCorrectly()
//    {
//        var parentPath = "electronics/computers";
//        var slug = "laptops";

//        var category = Category.Create(
//            name: "Laptops",
//            slug: slug,
//            level: 2,
//            isActive: true,
//            parentId: Guid.NewGuid(),
//            parentPath: parentPath).Value!;

//        category.Path.Should().Be("electronics/computers/laptops");
//    }

//    [Fact]
//    public void Create_WithNullOrEmptyName_Should_ThrowArgumentException()
//    {
//        Action actNull = () => Category.Create(null!, "slug", 0, true);
//        Action actEmpty = () => Category.Create("", "slug", 0, true);
//        Action actWhitespace = () => Category.Create("   ", "slug", 0, true);

//        actNull.Should().Throw<ArgumentException>()
//            .WithParameterName("name");

//        actEmpty.Should().Throw<ArgumentException>()
//            .WithParameterName("name");

//        actWhitespace.Should().Throw<ArgumentException>()
//            .WithParameterName("name");
//    }

//    [Fact]
//    public void Create_WithNegativeLevel_Should_ThrowArgumentException()
//    {
//        Action act = () => Category.Create("Name", "slug", -1, true);

//        act.Should().Throw<ArgumentException>()
//            .WithMessage("*level can't be negative*")
//            .WithParameterName("level");
//    }

//    [Fact]
//    public void Create_WithInactive_Should_SetIsActiveFalse()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, false);

//        category.IsActive.Should().BeFalse();
//    }

//    [Fact]
//    public void Create_Should_RaiseCategoryCreatedDomainEvent()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        var domainEvents = category.DomainEvents;
//        domainEvents.Should().ContainSingle();
//        domainEvents.First().Should().BeOfType<CategoryCreatedDomainEvent>();

//        var categoryCreatedEvent = (CategoryCreatedDomainEvent)domainEvents.First();
//        categoryCreatedEvent.Id.Should().Be(category.Id);
//    }

//    [Fact]
//    public void CreatePath_WithNoParentPath_Should_ReturnSlugOnly()
//    {
//        var result = Category.CreatePath(null, "electronics");

//        result.Should().Be("electronics");
//    }

//    [Fact]
//    public void CreatePath_WithParentPath_Should_CombineWithSlash()
//    {
//        var result = Category.CreatePath("electronics/computers", "laptops");

//        result.Should().Be("electronics/computers/laptops");
//    }

//    [Fact]
//    public void CreatePath_WithEmptyParentPath_Should_ReturnSlugOnly()
//    {
//        var result = Category.CreatePath("", "electronics");

//        result.Should().Be("electronics");
//    }

//    [Fact]
//    public void CreatePath_WithWhitespaceParentPath_Should_ReturnSlugOnly()
//    {
//        var result = Category.CreatePath("   ", "electronics");

//        result.Should().Be("electronics");
//    }

//    [Fact]
//    public void MoveCategory_WithValidData_Should_UpdateParentAndLevelAndPath()
//    {
//        var category = Category.Create("Laptops", "laptops", 2, true, Guid.NewGuid());
//        var newParentId = Guid.NewGuid();
//        var newParentPath = "electronics/computers";
//        var newParentLevel = (short)1;

//        category.MoveCategory(newParentId, newParentPath, newParentLevel);

//        category.ParentId.Should().Be(newParentId);
//        category.Level.Should().Be(2);
//        category.Path.Should().Be("electronics/computers/laptops");
//    }

//    [Fact]
//    public void MoveCategory_WithNullParentId_Should_KeepExistingParentId()
//    {
//        var existingParentId = Guid.NewGuid();
//        var category = Category.Create("Laptops", "laptops", 1, true, existingParentId);

//        category.MoveCategory(null, "new-path", 0);

//        category.ParentId.Should().Be(existingParentId);
//        category.Level.Should().Be(1);
//    }

//    [Fact]
//    public void MoveCategory_WithNegativeLevel_Should_ThrowArgumentException()
//    {
//        var category = Category.Create("Laptops", "laptops", 1, true);

//        Action act = () => category.MoveCategory(Guid.NewGuid(), "path", -1);

//        act.Should().Throw<ArgumentException>()
//            .WithMessage("*prevLevel can't be negative*")
//            .WithParameterName("parentLevel");
//    }

//    [Fact]
//    public void MoveCategory_Should_RaiseCategoryMovedDomainEvent()
//    {
//        var category = Category.Create("Laptops", "laptops", 1, true);
//        category.ClearDomainEvents();

//        category.MoveCategory(Guid.NewGuid(), "new-path", 0);

//        var domainEvents = category.DomainEvents;
//        domainEvents.Should().ContainSingle();
//        domainEvents.First().Should().BeOfType<CategoryMovedDomainEvent>();

//        var movedEvent = (CategoryMovedDomainEvent)domainEvents.First();
//        movedEvent.Id.Should().Be(category.Id);
//    }

//    [Fact]
//    public void MoveCategory_ToRoot_Should_UpdateToLevelOne()
//    {
//        var category = Category.Create("Category", "category", 5, true, Guid.NewGuid());

//        category.MoveCategory(Guid.NewGuid(), null, 0);

//        category.Level.Should().Be(1);
//        category.Path.Should().Be("category");
//    }

//    [Fact]
//    public void UpdateDetails_WithValidData_Should_UpdateNameAndDescription()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        var newName = "Electronics Updated";
//        var newDescription = "New description";

//        category.UpdateDetails(newName, newDescription);

//        category.Name.Should().Be(newName);
//        category.Description.Should().Be(newDescription);
//    }

//    [Fact]
//    public void UpdateDetails_WithNullDescription_Should_UpdateOnlyName()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true, description: "Original");
//        var newName = "Electronics Updated";

//        category.UpdateDetails(newName, null);

//        category.Name.Should().Be(newName);
//        category.Description.Should().Be("Original");
//    }

//    [Fact]
//    public void UpdateDetails_WithNullOrEmptyName_Should_ThrowArgumentException()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        Action actNull = () => category.UpdateDetails(null!, "description");
//        Action actEmpty = () => category.UpdateDetails("", "description");
//        Action actWhitespace = () => category.UpdateDetails("   ", "description");

//        actNull.Should().Throw<ArgumentException>().WithParameterName("name");
//        actEmpty.Should().Throw<ArgumentException>().WithParameterName("name");
//        actWhitespace.Should().Throw<ArgumentException>().WithParameterName("name");
//    }

//    [Fact]
//    public void UpdateDetails_Should_RaiseCategoryDetailsUpdatedDomainEvent()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        category.ClearDomainEvents();

//        category.UpdateDetails("Updated Name", "Updated Description");

//        var domainEvents = category.DomainEvents;
//        domainEvents.Should().ContainSingle();
//        domainEvents.First().Should().BeOfType<CategoryDetailsUpdatedDomainEvent>();

//        var updatedEvent = (CategoryDetailsUpdatedDomainEvent)domainEvents.First();
//        updatedEvent.Id.Should().Be(category.Id);
//    }

//    [Fact]
//    public void AddChild_WithValidCategory_Should_AddToChildren()
//    {
//        var parent = Category.Create("Parent", "parent", 0, true);
//        var child = Category.Create("Child", "child", 1, true, parent.Id);

//        parent.AddChild(child);

//        parent.Children.Should().ContainSingle();
//        parent.Children.First().Should().Be(child);
//    }

//    [Fact]
//    public void AddChild_MultipleChildren_Should_AddAllToCollection()
//    {
//        var parent = Category.Create("Parent", "parent", 0, true);
//        var child1 = Category.Create("Child1", "child1", 1, true, parent.Id);
//        var child2 = Category.Create("Child2", "child2", 1, true, parent.Id);

//        parent.AddChild(child1);
//        parent.AddChild(child2);

//        parent.Children.Should().HaveCount(2);
//        parent.Children.Should().Contain(new[] { child1, child2 });
//    }

//    [Fact]
//    public void AddVariantAttribute_WithValidAttribute_Should_AddToCollection()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);
//        var variantAttributeId = Guid.CreateVersion7();

//        category.AddVariantAttribute(
//            variantId: variantAttributeId,
//            isRequired: true,
//            displayOrder: 1);

//        category.CategoryVariantAttributes.Should().ContainSingle();
//        category.CategoryVariantAttributes.First().VariantAttributeId.Should().Be(variantAttributeId);
//        category.CategoryVariantAttributes.First().IsRequired.Should().Be(true);
//        category.CategoryVariantAttributes.First().DisplayOrder.Should().Be(1);
//    }

//    [Fact]
//    public void AddVariantAttribute_WithNullVairantId_Should_ThrowArgumentException()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        Action act = () => category.AddVariantAttribute(Guid.Empty, false, 1);

//        act.Should().Throw<ArgumentException>();
//    }

//    [Fact]
//    public void Children_Should_ReturnReadOnlyCollection()
//    {
//        var parent = Category.Create("Parent", "parent", 0, true);

//        parent.Children.Should().BeAssignableTo<IReadOnlyCollection<Category>>();
//    }

//    [Fact]
//    public void CategoryVariantAttributes_Should_ReturnReadOnlyCollection()
//    {
//        var category = Category.Create("Electronics", "electronics", 0, true);

//        category.CategoryVariantAttributes
//            .Should().BeAssignableTo<IReadOnlyCollection<CategoryVariantAttribute>>();
//    }

//    [Fact]
//    public void Create_WithAllParameters_Should_SetAllProperties()
//    {
//        var name = "Laptops";
//        var slug = "laptops";
//        var level = (short)2;
//        var isActive = true;
//        var parentId = Guid.NewGuid();
//        var description = "Laptop computers";
//        var parentPath = "electronics/computers";

//        var category = Category.Create(name, slug, level, isActive, parentId, description, parentPath);

//        category.Name.Should().Be(name);
//        category.Slug.Should().Be(slug);
//        category.Level.Should().Be(level);
//        category.IsActive.Should().BeTrue();
//        category.ParentId.Should().Be(parentId);
//        category.Description.Should().Be(description);
//        category.Path.Should().Be("electronics/computers/laptops");
//    }

//    [Fact]
//    public void MoveCategory_MultipleTimesSuccessively_Should_UpdateCorrectly()
//    {
//        var category = Category.Create("Category", "category", 0, true);

//        category.MoveCategory(Guid.NewGuid(), "path1", 0);
//        category.Level.Should().Be(1);
//        category.Path.Should().Be("path1/category");

//        category.MoveCategory(Guid.NewGuid(), "path2/subpath", 1);
//        category.Level.Should().Be(2);
//        category.Path.Should().Be("path2/subpath/category");

//        category.MoveCategory(Guid.NewGuid(), null, 0);
//        category.Level.Should().Be(1);
//        category.Path.Should().Be("category");
//    }

//    [Fact]
//    public void UpdateDetails_MultipleTimes_Should_KeepLatestValues()
//    {
//        var category = Category.Create("Name1", "slug", 0, true, description: "Desc1");

//        category.UpdateDetails("Name2", "Desc2");
//        category.Name.Should().Be("Name2");
//        category.Description.Should().Be("Desc2");

//        category.UpdateDetails("Name3", "Desc3");
//        category.Name.Should().Be("Name3");
//        category.Description.Should().Be("Desc3");
//    }
//}
