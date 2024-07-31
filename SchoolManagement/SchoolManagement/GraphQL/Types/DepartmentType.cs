using SchoolManagement.GraphQL.DataLoaders;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Types;

public class DepartmentType : ObjectType<Department>
{
    protected override void Configure(IObjectTypeDescriptor<Department> descriptor)
    {
        descriptor.Field(x => x.Teachers)
            .Description("This is the list of teachers in the department.")
            .Type<ListType<TeacherType>>()
            .ResolveWith<DepartmentResolvers>(x => x.GetTeachers(default, default, default));
    }
}

public class DepartmentResolvers
{
    // public async Task<List<Teacher>> GetTeachers([Parent] Department department,
    //     [Service] IDbContextFactory<AppDbContext>
    //         dbContextFactory)
    // {
    //     await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    //     var teachers = await dbContext.Teachers.Where(x =>
    //         x.DepartmentId == department.Id).ToListAsync();
    //     return teachers;
    // }

    public async Task<List<Teacher>> GetTeachers([Parent] Department department,
        TeachersByDepartmentIdDataLoader teachersByDepartmentIdDataLoader, CancellationToken cancellationToken)
    {
        var teachers = await teachersByDepartmentIdDataLoader.LoadAsync(department.Id, cancellationToken);
        return teachers.ToList();
    }
    //  The preceding code uses TeachersByDepartmentIdDataLoader to fetch the Teacher
    //  objects for the Department object. TeachersByDepartmentIdDataLoader will be
    //  injected by HotChocolate automatically.
}

//  DepartmentType has a Teachers field of the ListType<TeacherType> type.
//  Then, we use the ResolveWith() method to define the resolver. The resolver retrieves
//  the Teacher objects from the database using the DepartmentId property of the
//  Department object.