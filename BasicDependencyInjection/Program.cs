using BasicDependencyInjection;
using BasicDependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPostService, PostsService>();
// The preceding code utilizes the AddScoped() method, which indicates that the service is created
// once per client request and disposed of upon completion of the request.
builder.Services.AddScoped<IDemoService, DemoService>();
// builder.Services.AddSingleton<IDemoService, DemoService>();

// builder.Services.AddScoped<IScopedService, ScopedService>();
// builder.Services.AddTransient<ITransientService, TransientService>();
// builder.Services.AddSingleton<ISingletonService, SingletonService>();
// OR
// Group registration
builder.Services.AddLifetimeServices();

// builder.Services.AddKeyedScoped<IDataService, SqlDatabaseService>("sqlDatabaseService");
// builder.Services.AddKeyedScoped<IDataService, CosmosDatabaseService>("cosmosDatabaseService");
builder.Services.AddKeyedSingleton<IDataService, SqlDatabaseService>("sqlDatabaseService");
builder.Services.AddKeyedTransient<IDataService, CosmosDatabaseService>("cosmosDatabaseService");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var demoService = services.GetRequiredService<IDemoService>();
    var message = demoService.SayHello();
    Console.WriteLine(message);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();