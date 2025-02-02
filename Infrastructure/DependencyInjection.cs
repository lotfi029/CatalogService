using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbConfiguration(builder.Configuration);

        builder.Services.AddIdentityConfiguration();
    }
    public static IServiceCollection AddDbConfiguration(this IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("defaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }
    private static IServiceCollection AddIdentityConfiguration(this IServiceCollection services) 
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        //services.AddIdentityCore<ApplicationUser>()
        //    .AddRoles<ApplicationRole>() 
        //    .AddEntityFrameworkStores<ApplicationDbContext>() 
        //    .AddRoleManager<RoleManager<ApplicationRole>>() 
        //    .AddUserManager<UserManager<ApplicationUser>>() 
        //    .AddSignInManager<SignInManager<ApplicationUser>>()
        //    .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
        });


        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        //    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        //});

        return services;
    }
}
