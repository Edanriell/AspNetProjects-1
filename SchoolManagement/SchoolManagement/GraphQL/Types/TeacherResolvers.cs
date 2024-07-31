using SchoolManagement.GraphQL.DataLoaders;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Types;

// Alternative resolver
public class TeacherResolvers
{
    // public async Task<Department> GetDepartment([Parent] Teacher
    //     teacher, [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    // {
    //     // var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
    //     // await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    //     // var department = await dbContext.Departments
    //     //     .FindAsync(context.Parent<Teacher>().DepartmentId);
    //     // return department;
    //
    //     await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    //     var department = await dbContext.Departments.FindAsync(teacher.DepartmentId);
    //     return department;
    // }
    public async Task<Department> GetDepartment([Parent] Teacher
            teacher,
        DepartmentByTeacherIdBatchDataLoader
            departmentByTeacherIdBatchDataLoader, CancellationToken
            cancellationToken)
    {
        var department = await
            departmentByTeacherIdBatchDataLoader.LoadAsync(teacher.DepartmentId, cancellationToken);
        return department;
    }
    //  Instead of querying the database directly, the resolver uses
    //  DepartmentByTeacherIdBatchDataLoader to fetch the Department
    //  object for the DepartmentId property of the Teacher object. The
    //  DepartmentByTeacherIdBatchDataLoader will be injected by
    //  HotChocolate automatically
}