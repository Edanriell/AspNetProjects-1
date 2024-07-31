using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.DataLoaders;

public class DepartmentByTeacherIdBatchDataLoader(
    IDbContextFactory<AppDbContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : BatchDataLoader<Guid, Department>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Department>> LoadBatchAsync(IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var departments = await dbContext.Departments.Where(x => keys.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        return departments;
    }
}

//  The preceding code defines a data loader to fetch the batch data for the Department object. The
//  parent resolver, which is the teachers query, will get a list of Teacher objects. Each Teacher
//  object has a DepartmentId property. DepartmentByTeacherIdBatchDataLoader
//  will fetch the Department objects for the DepartmentId values in the list. The list of
//  the Department objects will be converted to a dictionary. The key of the dictionary is the
//  DepartmentId property and the value is the Department object. Then, the parent resolver
//  can map the Department object to the Teacher object in memory