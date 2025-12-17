var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("catalogservice")
    .WithDataVolume()
    .WithPgAdmin();

builder.AddProject<Projects.CatalogService_API>("catalogservice-api")
    .WithReference(postgres);

builder.Build().Run();
