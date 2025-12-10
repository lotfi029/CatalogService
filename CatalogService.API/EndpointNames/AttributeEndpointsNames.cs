namespace CatalogService.API.EndpointNames;

public class AttributeEndpointsNames
{
    public static string Tage => nameof(Attribute);

    public static string CreateAttribute => nameof(CreateAttribute);
    public static string CreateAttributeBulk => nameof(CreateAttributeBulk);
    public static string DeleteAttribute => nameof(DeleteAttribute);
    public static string GetAttributeById => nameof(GetAttributeById);
    public static string GetAllAttributes => nameof(GetAllAttributes);
    public static string GetAttributeByCode => nameof(GetAttributeByCode);
    public static string GetAttributeByType => nameof(GetAttributeByType);
    public static string ActivateAttribute => nameof(ActivateAttribute);
    public static string DeactivateAttribute => nameof(DeactivateAttribute);
    public static string UpdateAttributeOptions => nameof(UpdateAttributeOptions);
    public static string UpdateAttribteDetails => nameof(UpdateAttribteDetails);
}
