var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CatalogService_API>("catalogservice-api");

builder.Build().Run();
