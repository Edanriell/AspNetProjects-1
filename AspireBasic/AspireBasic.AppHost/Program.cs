using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<AspireBasic_ApiService>("apiservice");

builder.AddProject<AspireBasic_Web>("webfrontend")
   .WithExternalHttpEndpoints()
   .WithReference(apiService);

builder.Build().Run();