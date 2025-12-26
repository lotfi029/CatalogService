using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.CategoryVariants.Queries;
using CatalogService.Application.Features.ProductAttributes.Queries;
using CatalogService.Application.Features.ProductCategories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Features.ProductVariants.Queries;
using CatalogService.Application.Features.VariantAttributes.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.DomainEvents;
using CatalogService.Infrastructure.Persistence;
using CatalogService.Infrastructure.Persistence.ConnectionStrings;
using CatalogService.Infrastructure.Persistence.Dapper;
using CatalogService.Infrastructure.Persistence.Dapper.Queries;
using CatalogService.Infrastructure.Persistence.Repositories;
using CatalogService.Infrastructure.Search.Elasticsearch;
using CatalogService.Infrastructure.Search.Elasticsearch.IndexManager;
using CatalogService.Infrastructure.Search.Elasticsearch.Services;
using CatalogService.Infrastructure.Search.ElasticSearch;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CatalogService.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration) => services
            .AddPersistence(configuration)
            .AddElasticSearchSearvices(configuration)
            .AddServices();

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionStringOrThrow();
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryVariantAttributeRepository, CategoryVariantAttributeRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
        
        services.AddScoped<IAttributeRepository, AttributeRepository>();
        services.AddScoped<IVariantAttributeRepository, VariantAttributeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        
        services.AddHealthChecks()
            .AddNpgSql(name: "ApplicationDb", connectionString: connectionString!);


        services.Configure<DapperOptions>(opt =>
        {
            opt.ConnectionString = connectionString!;
        });
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<IProductQueries, ProductQueries>();
        services.AddScoped<IProductCategoryQueries, ProductCategoryQueries>();
        services.AddScoped<IProductAttributeQueries, ProductAttributeQueries>();
        services.AddScoped<IProductVariantQueries, ProductVariantQueries>();
        services.AddScoped<IAttributeQueries, AttributeQueries>();
        services.AddScoped<ICategoryVariantAttributeQueries, CategoryVariantAttributeQueries>();
        services.AddScoped<IVariantAttributeQueries, VariantAttributeQueries>();

        DapperConfiguration.Configure();


        return services;
    }
    private static IServiceCollection AddElasticSearchSearvices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ElasticsearchSettings>()
            .BindConfiguration(ElasticsearchSettings.SectionName)
            .ValidateOnStart();

        services.AddSingleton(options =>
        {
            var elasticOptions = options.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;
            var logger = options.GetRequiredService<ILogger<ElasticsearchClient>>();
            var settings = new ElasticsearchClientSettings(new Uri(elasticOptions.Uri));

            if (!string.IsNullOrEmpty(elasticOptions.Username) && !string.IsNullOrEmpty(elasticOptions.Password))
            {
                settings = settings.Authentication(new BasicAuthentication(elasticOptions.Username, elasticOptions.Password));
            }

            settings
                .RequestTimeout(TimeSpan.FromSeconds(elasticOptions.RequestTimeout))
                .DisableDirectStreaming(elasticOptions.EnableDebugMode);

            if (elasticOptions.EnableDebugMode)
            {
                settings.OnRequestCompleted(response =>
                {
                    if (response.DebugInformation is not null)
                    {
                        logger.LogDebug("Elasticsearch request: {DebugInformation}", response.DebugInformation);
                    }
                });
            }
            return new ElasticsearchClient(settings);
        });
        services.AddSingleton<IElasticsearchIndexManager, ElasticsearchIndexManager>();
        services.AddScoped<IProductSearchService, ProductSearchService>();
        services.AddScoped<IAttributeSearchService, AttributeSearchService>();
        services.AddScoped<ICategorySearchService, CategorySearchService>();
        
        return services;
    }
    public static async Task InitialElasticsearch(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var indexManager = scope.ServiceProvider.GetRequiredService<IElasticsearchIndexManager>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<IElasticsearchIndexManager>>();

        logger.LogInformation("Initializing Elasticsearch indices...");

        await indexManager.CreateProductIndexAsync();
        await indexManager.CreateCategoryIndexAsync();
        await indexManager.CreateAttributeIndexAsync();
        await indexManager.CreateVariantAttributeIndexAsync();

        logger.LogInformation("Elasticsearch indices initialized successfully");
    }
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        return services;
    }
}
