using BasicHttpClient;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
	client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
	client.DefaultRequestHeaders.Add(HeaderNames.Accept,    "application/json");
	client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientDemo");
});

builder.Services.AddHttpClient<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();