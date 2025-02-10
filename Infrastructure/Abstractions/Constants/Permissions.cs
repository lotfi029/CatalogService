namespace Infrastructure.Abstractions.Constants;

public class Permissions 
{
    public static string Type { get; } = "permissions";

    public const string ProductGet = "product:get";
    public const string ProductAdd = "product:add";
    public const string ProductUpdate = "product:update";
    public const string ProductDelete = "product:delete";

    public const string CategoryGet = "category:get";
    public const string CategoryAdd = "category:add";
    public const string CategoryUpdate = "category:update";
    public const string CategoryDelete = "category:delete";

    public const string ActionGet = "action:get";
    public const string ActionAdd = "action:add";
    public const string ActionUpdate = "action:update";
    public const string ActionDelete = "action:delete";

    public static IList<string> GetPermissions =>
        typeof(Permissions).GetType().GetFields().Select(f => f.GetValue(f) as string).ToList()!;
}