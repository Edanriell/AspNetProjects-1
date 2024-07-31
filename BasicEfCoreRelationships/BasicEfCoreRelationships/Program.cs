using System.Text.Json.Serialization;
using BasicEfCoreRelationships.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SampleDbContext>();
// If simmilar error happens 
// An unhandled exception has occurred while executing the request.
// System.Text.Json.JsonException: A possible object cycle was
// detected. This can either be due to a cycle or if the object
// depth is larger than the maximum allowed depth of 32. Consider
// using ReferenceHandler.Preserve on JsonSerializerOptions to
// support cycles. Path: $.InvoiceItems.Invoice.InvoiceItems.
// Invoice.InvoiceItems.Invoice.InvoiceItems.Invoice.InvoiceItems.
// Invoice.InvoiceItems.Invoice.InvoiceItems.Invoice.InvoiceItems.
// Invoice.InvoiceItems.Invoice.InvoiceItems.Invoice.InvoiceItems.
// We must .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
// Another way to fix the exception is to use the [JsonIgnore] attribute to decorate the
// Invoice property in the InvoiceItem class. But if you have many entities with such a
// relationship, it is tedious to decorate all of them.

// System.Text.Json is a new JSON serialization framework provided since .NET Core 3.0. It
// is faster and more efficient than Newtonsoft.Json. It is also the default JSON serialization
// framework in ASP.NET Core 3.0 and later versions. It is recommended to use System.Text.
// Json instead of Newtonsoft.Json in new projects.

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