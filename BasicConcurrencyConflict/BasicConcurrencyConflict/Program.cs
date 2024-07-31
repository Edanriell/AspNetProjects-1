using System.Text.Json.Serialization;
using BasicConcurrencyConflict.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SampleDbContext>();

// Reset the database
using var scope = builder.Services.BuildServiceProvider().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Pessimistic concurrency control: This uses database locks to prevent multiple clients from
// updating the same entity at the same time. When a client tries to update an entity, it will first
// acquire a lock on the entity. If the lock is acquired successfully, only this client can update the
// entity, and all other clients will be blocked from updating the entity until the lock is released.
// However, this approach may result in performance issues when the number of concurrent
// clients is large because managing locks is expensive. EF Core does not have built-in support
// for pessimistic concurrency control.

// Optimistic concurrency control: This way does not involve locks; instead, a version column is
// used to detect concurrency conflicts. When a client tries to update an entity, it will first get the
// value of the version column, and then compare this value with the old value when updating the
// entity. If the value of the version column is the same, it means that no other client has updated
// the entity. In this case, the client can update the entity. But if the value of the version column is
// different from the old value, it means that another client has updated the entity. In this case, EF
// Core will throw an exception to indicate the concurrency conflict. The client can then handle
// the exception and retry the update operation.