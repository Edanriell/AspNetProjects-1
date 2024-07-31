using BasicCaching;
using BasicCaching.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddHostedService<CategoriesCacheBackgroundService>();

builder.Services.AddMemoryCache();

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = "localhost:6379";
	options.InstanceName  = "CachingDemo";
});

builder.Services.AddOutputCache(options =>
{
	options.AddBasePolicy(x => x.Cache());
	options.AddPolicy("Expire600",  x => x.Expire(TimeSpan.FromSeconds(600)));
	options.AddPolicy("Expire3600", x => x.Expire(TimeSpan.FromSeconds(3600)));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseOutputCache();

app.Run();