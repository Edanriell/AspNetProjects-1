using AdvancedEfCore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// For performance boost
// For most applications, DbContext pooling is not necessary. You should enable DbContext
// pooling only if you have a high-throughput application. Therefore, before enabling DbContext
// pooling, it is important to test your application’s performance with and without it to see whether
// there’s any noticeable improvement.
var useDbContextPooling = true;
if (useDbContextPooling)
    builder.Services.AddDbContextPool<InvoiceDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
else
    builder.Services.AddDbContext<InvoiceDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

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