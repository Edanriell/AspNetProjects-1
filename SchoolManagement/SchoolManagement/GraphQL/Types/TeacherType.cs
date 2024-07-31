using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Types;

public class TeacherType : ObjectType<Teacher>
{
    protected override void
        Configure(IObjectTypeDescriptor<Teacher> descriptor)
    {
        // descriptor.Field(x => x.Department)
        //     .Name("department")
        //     .Description("This is the department to which the teacher belongs.")
        //     .Resolve(async context =>
        //     {
        //         var department = await context.Service<AppDbContext>().Departments
        //             .FindAsync(context.Parent<Teacher>().DepartmentId);
        //         return department;
        //     });

        //  The preceding code defines a GetDepartment() method that takes a Teacher object as the
        //  parent object and returns the Department object. Then, we can use the ResolveWith() method
        //  to define the resolver in the TeacherType class
        //  Now, the logic of the resolver is moved to a separate class. This approach is more flexible when the
        //  resolver is complex. But for simple resolvers, we can use the delegate method directly.  
        // descriptor.Field(x => x.Department)
        //     .Description("This is the department to which the teacher belongs.")
        //     .ResolveWith<TeacherResolvers>(x => x.GetDepartment(default, default));
        // If we use (DbContextKind.Pooled) we need also to update our resolvers
        descriptor.Field(x => x.Department)
            .Description("This is the department to which the teacher belongs.")
            // .Resolve(async context =>
            // {
            //     var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
            //     await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            //     var department = await dbContext.Departments
            //         .FindAsync(context.Parent<Teacher>().DepartmentId);
            //     return department;
            // });
            // For using the resolver, it could be put in one file or be separeted
            .ResolveWith<TeacherResolvers>(x => x.GetDepartment(default, default, default));
    }
}

//  The TeacherType class inherits from the ObjectType<Teacher> class, which has
//  a Configure() method to configure the GraphQL object and specify how to resolve the
//  fields. In the preceding code, we use the code-first approach to define the Department field
//  of TeacherType. The Name method is used to specify the name of the field. If the name of
//  the field is the same as the name of the property following the convention, we can omit the
//  Name method. By convention, the Department field will be converted to the department
//  field in the schema. Then, we use the Description method to define the description of the
//  field. The description will be shown in the GraphQL IDE.
//  Then, we use the Resolve() method to define the resolver. The resolver retrieves the
//  Department object from the database using the DepartmentId property of the Teacher
//  object. Note that we use the context.Parent<Teacher>() method to get the Teacher
//  object because the Teacher object is the parent object of the Department object.

// public class TeacherResolvers
// {
//     // The following code is commented out because it is replaced by the data loader.
//     //public async Task<Department> GetDepartment([Parent] Teacher teacher, [Service] IDbContextFactory<AppDbContext> dbContextFactory)
//     //{
//     //    await using var dbContext = await dbContextFactory.CreateDbContextAsync();
//     //    var department = await dbContext.Departments.FindAsync(teacher.DepartmentId);
//     //    return department;
//     //}
//
//     public async Task<Department> GetDepartment([Parent] Teacher teacher,
//         DepartmentByTeacherIdBatchDataLoader departmentByTeacherIdBatchDataLoader, CancellationToken cancellationToken)
//     {
//         var department = await departmentByTeacherIdBatchDataLoader.LoadAsync(teacher.DepartmentId, cancellationToken);
//         return department;
//     }
// }