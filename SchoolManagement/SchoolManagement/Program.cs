using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.GraphQL.Filters;
using SchoolManagement.GraphQL.Mutations;
using SchoolManagement.GraphQL.Queries;
using SchoolManagement.GraphQL.Types;
using SchoolManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Is required for (DbContextKind.Pooled) option
builder.Services.AddPooledDbContextFactory<AppDbContext>(options
    =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// For dependency injection
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ISchoolRoomService, SchoolRoomService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IFurnitureService, FurnitureService>();
builder.Services.AddScoped<IStudentService, StudentService>();
//  HotChocolate uses the same approach to register the services as ASP.NET Core, but injecting the
//  services is a little different. In ASP.NET Core, we can inject the services into the controller constructor,
//  while HotChocolate does not recommend constructor injection. Instead, HotChocolate recommends
//  using the method-level injection. First, the GraphQL type definitions are singleton objects. If we use
//  constructor injection, the services will be injected as singleton objects as well. This is not what we
//  want. Second, sometimes HotChocolate needs to synchronize the resolvers to avoid concurrency
//  issues. If we use constructor injection, HotChocolate cannot control the lifetime of the services. Note
//  that this applies to the HotChocolate GraphQL types and resolvers only. For other services, we can
//  still use constructor injection

// The preceding code registers the GraphQL server and adds the Query type to the schema.
// builder.Services
//     .AddGraphQLServer()
//     .AddQueryType<Query>()
//     // To add mutations we must
//     .AddMutationType<Mutation>();

//  We use QueryType to replace the Query type we defined previously so that we can use
//  the resolver to retrieve the Department object when the department field is requested.
builder.Services
    .AddGraphQLServer()
    //  Added to resolve concurrency issues
    //  The RegisterDbContext<TDbContext>() method can specify how DbContext should be
    //  injected. There are three options:
    //      • DbContextKind.Synchronized: This is to ensure that DbContext is never used
    //      concurrently. DbContext is still injected as a scoped service.
    //      • DbContextKind.Resolver: This way will resolve the scoped DbContext for each resolver.
    //      This option is the default configuration. From the perspective of the resolver, DbContext is a
    //      transient service, so HotChocolate can execute multiple resolvers concurrently without any issues.
    //      • DbContextKind.Pooled: This mechanism will create a pool of DbContext instances. It
    //      leverages the DbContextPool feature of EF Core. HotChocolate will resolve DbContext
    //      from the pool for each resolver. When the resolver is completed, DbContext will be returned
    //      to the pool. In this way, DbContext is also like a transient service for each resolver, so
    //      HotChocolate can parallelize the resolvers as well.
    // .RegisterDbContext<AppDbContext>()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)
    //  The preceding code registers AppDbContext as a pooled service using
    //  the AddPooledDbContextFactory() method. Then, we use the
    //  RegisterDbContext() method to register AppDbContext as a pooled service for
    //  HotChocolate resolvers.
    //  If we have many services in the project, using the attribute for each service is tedious. HotChocolate
    //  provides a RegisterServices() method to simplify the injection.
    // .RegisterService<ITeacherService>()
    .RegisterService<ITeacherService>(ServiceKind.Resolver)
    .AddQueryType<QueryType>()
    .AddType<LabRoomType>()
    .AddType<ClassroomType>()
    .AddType<CustomStudentFilterType>()
    .AddFiltering()
    .AddSorting()
    .AddMutationType<Mutation>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// The preceding code maps the GraphQL endpoint to the /graphql URL
app.MapGraphQL();

app.MapGraphQLVoyager("/voyager");

app.Run();