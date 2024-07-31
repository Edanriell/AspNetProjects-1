var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

// We need to use the dbcontext scaffold command to generate the entity classes
// and DbContext from the database schema. This command needs the connection string of
// the database and the name of the database provider. We can run the following command in
// the terminal:
// dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Initial Catalog=EfCoreRelationshipsDemoDb;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer
// By default, the generated files will be placed in the current directory. We can use the --contextdir and --output-dir options to specify the output directory of the DbContext and
// entity classes. For example, we can run the following command to generate the DbContext
// and entity classes in the Data and Models folders, respectively:
// dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Initial Catalog=EfCoreDb;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer--context-dir Data --output-dir Models
// The default name of the DbContext class will be the same as the database name, such as
// EfCoreDbContext.cs. We can also change the name of the
// DbContext class by using the --context option. For example, we can run the following
// command to change the name of the DbContext class to AppDbContext:
// dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Initial Catalog=EfCoreRelationshipsDemoDb;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --context-dir Data --output-dir Models --context AppDbContext
// If the command executes successfully, we will see the generated files in the Data and Models folders.

// In the OnModelCreating method, we can see the entity classes and their relationships have been
// configured in Fluent API style. If you prefer to use data annotations, you can use the --dataannotations option when you run the dbcontext scaffold command.

// Keep in mind that the generated code is just a starting point. Some models or properties may not be
// represented correctly in the database. For example, if your models have inheritance, the generated
// code will not include the base class because the base class is not represented in the database. Also,
// some column types may not be able to be mapped to the corresponding CLR types. For example, the
// Status column in the Invoice table is of the nvarchar(16) type, which will be mapped to
// the string type in the generated code, instead of the Status enum type.

