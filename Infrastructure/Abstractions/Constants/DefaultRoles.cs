namespace Infrastructure.Abstractions.Constants;
public static class DefaultRoles
{
    public partial class Admin
    {
        public const string Name = nameof(Admin);
        public const string Id = "019409cc-7157-71a4-99d5-e295c82679db";
        public const string ConcurrencyStamp = "019409cc-285c-796f-ba84-6d2d43a19e2e";
    }
    public partial class NormalUser
    {
        public const string Name = nameof(NormalUser);
        public const string Id = "019409cd-931b-7028-b668-bbc65d9213e0";
        public const string ConcurrencyStamp = "019409cd-7700-71c9-add3-699453281dc4";
    }
}
