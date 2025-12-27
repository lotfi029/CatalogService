namespace CatalogService.Infrastructure.Search.Elasticsearch;

internal sealed class ElasticsearchIndexNames
{
    public static string ProductPostfixIndex => "products";
    public static string CategoryPostfixIndex => "categories";
    public static string AttributePostfixIndex => "attributes";
    public static string VariantAttributeDefinitionPostfixIndex => "variant-attributes";
}
