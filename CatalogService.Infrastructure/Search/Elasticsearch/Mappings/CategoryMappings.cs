using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.DTOs.CategoryVariantAttributes;
using Elastic.Clients.Elasticsearch.Mapping;

namespace CatalogService.Infrastructure.Search.Elasticsearch.Mappings;

public sealed class CategoryMappings
{
    public static Properties PropertiesMapping => new Properties<CategoryDetailedResponse>
    {
        { e => e.Id, new KeywordProperty() },
        { e => e.Slug, new KeywordProperty() },
        { e => e.ParentId, new KeywordProperty() },
        {
            e => e.Name, new TextProperty
            {
                Fields = new Properties
                {
                    { "keyword", new KeywordProperty() }
                }
            }
        },
        { e => e.Description, new TextProperty() },
        { e => e.Level, new ShortNumberProperty() },
        { e => e.Path, new TextProperty() },
        { e => e.IsActive, new BooleanProperty() },
        {
            e => e.Variants, new NestedProperty
            {
                Properties = new Properties<CategoryVariantForCategoryResponse>
                {
                    { v => v.VariantAttributeId, new KeywordProperty() },
                    {
                        v => v.VariantAttributeName, new TextProperty
                        {
                            Fields = new Properties
                            {
                                { "keyword", new KeywordProperty() }
                            }
                        }
                    },
                    { v => v.Code, new KeywordProperty() },
                    { v => v.Datatype, new KeywordProperty() },
                    { v => v.DisplayOrder, new ShortNumberProperty() },
                    { v => v.IsRequired, new BooleanProperty() }
                }
            }
        }

    };
}