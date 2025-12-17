using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.DTOs.ProductCategories;
using CatalogService.Application.DTOs.Products;
using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Domain.JsonProperties;
using Elastic.Clients.Elasticsearch.Mapping;

namespace CatalogService.Infrastructure.Search.Elasticsearch.Mappings;

public sealed class ProductMappings
{
    public static Properties PropertiyMappings => new Properties<ProductDetailedResponse>
    {
        { e => e.Id, new KeywordProperty() },
        { e => e.Status, new KeywordProperty() },
        { e => e.VendorId, new KeywordProperty() },
        {
            e => e.Name, new TextProperty
            {
                Analyzer = "autocomplete",
                Fields = new Properties
                {
                    { "keyword", new KeywordProperty() }
                }
            }
        },
        { e => e.Description, new TextProperty() },
        { e => e.IsActive, new BooleanProperty() },
        { e => e.CreatedAt, new DateProperty() },
        { e => e.LastUpdateAt, new DateProperty() },
        {
            e => e.ProductCategories, new NestedProperty
            {
                Properties = new Properties<ProductCategoryResponse>
                {
                    { c => c.CategoryId, new KeywordProperty() },
                    {
                        c => c.CategoryName, new TextProperty
                        {
                            Fields = new Properties
                            {
                                { "keyword", new KeywordProperty() }
                            }
                        }
                    },
                    { c => c.CategorySlug, new KeywordProperty() },
                    { c => c.IsPrimary, new BooleanProperty() }
                }
            }
        },
        {
            e => e.ProductAttributes, new NestedProperty
            {
                Properties = new Properties<ProductAttributeResponse>
                {
                    { a => a.AttributeId, new KeywordProperty() },
                    {
                        a => a.AttributeName, new TextProperty
                        {
                            Fields = new Properties
                            {
                                { "keyword", new KeywordProperty() }
                            }
                        }
                    },
                    { a => a.AttributeCode, new KeywordProperty() },
                    { a => a.IsFilterable, new BooleanProperty() },
                    { a => a.IsSearchable, new BooleanProperty() },
                    {
                        a => a.AttributeValue, new TextProperty
                        {
                            Fields = new Properties
                            {
                                { "keyword", new KeywordProperty() }
                            }
                        }
                    }
                }
            }
        },
        {
            e => e.ProductVariants, new NestedProperty
            {
                Properties = new Properties<ProductVariantResponse>
                {
                    { v => v.ProductVariantId, new KeywordProperty() },
                    { v => v.Sku, new KeywordProperty() },
                    { v => v.Currency, new KeywordProperty() },
                    { v => v.Price, new FloatNumberProperty() },
                    { v => v.CompareAtPrice, new FloatNumberProperty()},
                    {
                        v => v.VariantAttributes, new NestedProperty
                        {
                            Properties = new Properties<ProductVariantsOption>
                            {
                                {
                                    o => o.Variants, new NestedProperty
                                    {
                                        Properties = new Properties<VariantAttributeItem>
                                        {
                                            { i => i.Key, new KeywordProperty() },
                                            {
                                                i => i.Value, new TextProperty
                                                {
                                                    Fields = new Properties
                                                    {
                                                        { "keyword", new KeywordProperty() }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    };
}
