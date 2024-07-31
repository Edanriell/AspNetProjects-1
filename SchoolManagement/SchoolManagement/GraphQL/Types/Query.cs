using HotChocolate.Data.Filters;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.GraphQL.Filters;
using SchoolManagement.GraphQL.Sorts;
using SchoolManagement.GraphQL.Types;
using SchoolManagement.Models;
using SchoolManagement.Services;

namespace SchoolManagement.GraphQL.Queries;

// The Query class will be used to define the queries that can be executed by the client. It has
// one method named GetTeachers(), which returns a list of teachers.
public class Query
{
    public List<TeacherType> Teachers { get; set; } = new();

    public TeacherType? Teacher { get; set; } = new();

    public List<DepartmentType> Departments { get; set; } = new();

    public DepartmentType? Department { get; set; } = new();

    public List<SchoolRoomType> SchoolRooms { get; set; } = new();

    public List<SchoolItemType> SchoolItems { get; set; } = new();

    public List<Student> Students { get; set; } = new();

    public List<Student> StudentsWithCustomFilter { get; set; } = new();
    //public async Task<List<Teacher>> GetTeachers([Service] AppDbContext context) =>
    //    await context.Teachers.Include(x => x.Department).ToListAsync();

    //public async Task<Teacher?> GetTeacher(Guid id, [Service] AppDbContext context) =>
    //    await context.Teachers.FindAsync(id);

    //public async Task<List<Course>> GetCourses([Service] AppDbContext context) =>
    //    await context.Courses.ToListAsync();

    // The following code uses the Service attribute to inject the ITeacherService service.
    //public async Task<List<Teacher>> GetTeachersWithDI([Service(ServiceKind.Resolver)] ITeacherService teacherService) =>
    //    await teacherService.GetTeachersAsync();

    // The following code does not use the Service attribute to inject the ITeacherService service because the service is registered in the GraphQL server.
    public async Task<List<Teacher>> GetTeachersWithDI(ITeacherService teacherService)
    {
        return await teacherService.GetTeachersAsync();
    }

    // public async Task<List<Teacher>> GetTeachers([Service] AppDbContext context)
    // {
    //     return await context.Teachers.ToListAsync();
    // }

    //  But this is not the best way to do it. Remember that GraphQL allows clients to specify the data they
    //  need. If the query does not specify the department field, the Department object will still be
    //  retrieved from the database. This is not efficient. We should only retrieve the Department object
    //  when the department field is specified in the query. That leads us to the concept of resolvers.
    //  A resolver is a function that is used to retrieve data from somewhere for a specific field. The resolver
    //  is executed when the field is requested in the query. The resolver can fetch data from a database, a
    //  web API, or any other data source. It will drill down the graph to retrieve the data for the field. For
    //  example, when the department field is requested in the teachers query, the resolver will retrieve
    //  the Department object from the database. But when the query does not specify the department
    //  field, the resolver will not be executed. This can avoid unnecessary database queries.

    // public async Task<List<Teacher>> GetTeachers([Service] AppDbContext
    //     context)
    // {
    //     return await context.Teachers.Include(x => x.Department).ToListAsync();
    // }

    // public async Task<Teacher?> GetTeacher(Guid id, [Service] AppDbContext context)
    // {
    //     return await context.Teachers.FindAsync(id);
    // }
    // public async Task<List<Teacher>> GetTeachersWithDI([Service] ITeacherService teacherService)
    // {
    //     return await teacherService.GetTeachersAsync();
    // }
    // Note that the Service attribute is from the HotChocolate namespace, not the Microsoft.
    // AspNetCore.Mvc namespace. With this attribute, ITeacherService will be injected into the
    // teacherService parameter automatically.
    // When we injected using RegisterService
    // public async Task<List<Teacher>> GetTeachersWithDI(ITeacherService
    //     teacherService)
    // {
    //     return await teacherService.GetTeachersAsync();
    // }
    // public async Task<List<Teacher>>
    //     GetTeachersWithDI([Service(ServiceKind.Resolver)] ITeacherService
    //         teacherService)
    // {
    //     return await teacherService.GetTeachersAsync();
    // }
    //  The preceding code specifies the ServiceKind as ServiceKind.Resolver. So,
    //  ITeacherService will be resolved for each resolver scope.
}

public class QueryType : ObjectType<Query>
{
    protected override void
        Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor.Field(x => x.Teacher)
            .Description("This is the teacher in the school.")
            .Type<TeacherType>()
            .Argument("id", a => a.Type<NonNullType<UuidType>>())
            .Resolve(async context =>
            {
                var id = context.ArgumentValue<Guid>("id");
                var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();
                var teacher = await dbContext.Teachers.FindAsync(id);
                return teacher;
            });

        // descriptor.Field(x => x.Teacher)
        //     .Name("teacher")
        //     .Description("This is the teacher in the school.")
        //     .Type<TeacherType>()
        //     .Argument("id", a =>
        //         a.Type<NonNullType<UuidType>>())
        //     .Resolve(async context =>
        //     {
        //         var id = context.ArgumentValue<Guid>("id");
        //         var teacher = await context.Service<AppDbContext>().Teachers.FindAsync(id);
        //         return teacher;
        //     });

        //  The preceding code defines the Teachers field of QueryType. It uses ListType<TeacherType>
        //  to define a list of TeacherType. Then, it uses the Resolve() method to define the resolver. The
        //  resolver retrieves all the Teacher objects from the database. This code is similar to the teacher
        //  field we defined previously. However, it retrieves a list of TeacherType objects instead of a single
        //  TeacherType object. As TeacherType has a resolver for the Department field, we can retrieve
        //  the Department object for each TeacherType object.
        // descriptor.Field(x => x.Teachers)
        //     .Name("teachers") // This configuration can be omitted if the name of the field is the same as the name of the property.
        //     .Description("This is the list of teachers in the school.")
        //     // .Type<ListType<TeacherType>>()
        //     // .Resolve(async context =>
        //     // {
        //     //     var teachers = await context.Service<AppDbContext>().Teachers.ToListAsync();
        //     //     return teachers;
        //     // }); 
        //     .Type<ListType<TeacherType>>()
        //     .Resolve(async context =>
        //     {
        //         // If we are using (DbContextKind.Pooled) 
        //         // We need adjust our query
        //         //  The preceding code uses IDbContextFactory<TDbContext> to create a new
        //         //  AppDbContext instance for each resolver. Then, it retrieves the Teacher objects from the
        //         //  database using the new AppDbContext instance.
        //         //  One thing to note is that we need to use the await using statement to dispose of the
        //         //  AppDbContext instance after the resolver is completed in order to return the AppDbContext
        //         //  instance to the pool.
        //         // var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
        //         // await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        //         // var teachers = await dbContext.Teachers.ToListAsync();
        //         // return teachers;
        //         var teacherService = context.Service<ITeacherService>();
        //         var teachers = await teacherService.GetTeachersAsync();
        //         return teachers;
        //         //  The preceding code uses the context.Service<T>() method to get ITeacherService from
        //         //  the context object, which is similar to injecting IDbContextFactory<AppDbContext>
        //     });

        descriptor.Field(x => x.Teachers)
            .Description("This is the list of teachers in the school.")
            .Type<ListType<TeacherType>>()
            .Resolve(async context =>
            {
                var teacherService = context.Service<ITeacherService>();
                var teachers = await teacherService.GetTeachersAsync();
                return teachers;
            });

        descriptor.Field(x => x.Departments)
            .Description("This is the list of departments in the school.")
            .Type<ListType<DepartmentType>>()
            .Resolve(async context =>
            {
                var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();
                var departments = await dbContext.Departments.ToListAsync();
                return departments;
            });

        descriptor.Field(x => x.Department)
            .Description("This is the department in the school.")
            .Type<DepartmentType>()
            .Argument("id", a => a.Type<NonNullType<UuidType>>())
            .Resolve(async context =>
            {
                var id = context.ArgumentValue<Guid>("id");
                var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();
                var department = await dbContext.Departments.FindAsync(id);
                return department;
            });

        descriptor.Field(x => x.SchoolRooms)
            .Description("This is the list of school rooms in the school.")
            .Type<ListType<SchoolRoomType>>()
            .Resolve(async context =>
            {
                var service = context.Service<ISchoolRoomService>();
                var schoolRooms = await service.GetSchoolRoomsAsync();
                return schoolRooms;
            });

        //  In the preceding code, we use the Service() method to get ISchoolRoomService from
        //  the context object. Then, we use the GetSchoolRoomsAsync() method to retrieve the
        //  list of ISchoolRoom objects. The result includes both LabRoom and Classroom objects

        descriptor.Field(x => x.SchoolItems)
            .Description("This is the list of school items in the school.")
            .Type<ListType<SchoolItemType>>()
            .Resolve(async context =>
            {
                var equipmentService = context.Service<IEquipmentService>();
                var furnitureService = context.Service<IFurnitureService>();
                var equipmentTask = equipmentService.GetEquipmentListAsync();
                var furnitureTask = furnitureService.GetFurnitureListAsync();
                await Task.WhenAll(equipmentTask, furnitureTask);
                var schoolItems = new List<object>();
                schoolItems.AddRange(equipmentTask.Result);
                schoolItems.AddRange(furnitureTask.Result);
                return schoolItems;
            });

        // descriptor.Field(x => x.Students)
        //     .Description("This is the list of students in the school.")
        //     // .UseFiltering()
        //     // .UseSorting()
        //     // .UsePaging()
        //     //  HotChocolate supports offset-based pagination as well. To use offset-based pagination, we need to
        //     //  use the UseOffsetPaging() method instead of the UsePaging() method. 
        //     // .UseOffsetPaging()
        //     // .UseOffsetPaging(options: new PagingOptions()
        //     // {
        //     //     MaxPageSize = 20,
        //     //     DefaultPageSize = 5,
        //     //     IncludeTotalCount = true
        //     // })
        //     .UsePaging(options: new PagingOptions
        //     {
        //         MaxPageSize = 20,
        //         DefaultPageSize = 5,
        //         IncludeTotalCount = true
        //     })
        //     //  Cursor-based pagination: This is the default pagination in HotChocolate. It uses a cursor to
        //     //  indicate the current position in the list. The cursor is usually an ID or a timestamp, which is
        //     //  opaque to the client.
        //     //  Offset-based pagination: This pagination uses the skip and take arguments to paginate
        //     //  the list.
        //     //  In GraphQL, the connection type is a standard way to paginate the list. The
        //     //  StudentsConnection type includes three fields: pageInfo, edges, and nodes. The
        //     //  nodes field is a list of the Student objects. The edges and pageInfo fields are defined
        //     //  in the StudentsEdge and PageInfo types.
        //     //  The result contains a cursor field for each Student object. The cursor field is an opaque
        //     //  string, which is used to indicate the current position in the list. The pageInfo field indicates
        //     //  whether there are more pages. In this case, the hasNextPage field is true, which means
        //     //  there are more pages.
        //     .UseFiltering<StudentFilterType>()
        //     .UseSorting<StudentSortType>()
        //     .Resolve(async context =>
        //     {
        //         var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
        //         var dbContext = await dbContextFactory.CreateDbContextAsync();
        //         var students = dbContext.Students.AsQueryable();
        //         return students;
        //     });

        descriptor.Field(x => x.Students)
            .Description("This is the list of students in the school.")
            //.UsePaging(options: new PagingOptions()
            //{
            //    MaxPageSize = 5,
            //    DefaultPageSize = 5,
            //    IncludeTotalCount = true
            //})
            //.UseOffsetPaging(options: new PagingOptions()
            //{
            //    MaxPageSize = 5,
            //    DefaultPageSize = 5,
            //    IncludeTotalCount = true
            //})
            .UseFiltering<StudentFilterType>()
            .UseSorting<StudentSortType>()
            .Resolve(async context =>
            {
                // The following code uses the DbContext directly and returns IQueryable<Student>.
                var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
                var dbContext = await dbContextFactory.CreateDbContextAsync();
                var students = dbContext.Students.AsQueryable();

                // We can also use the IStudentService service to get the list of students.
                //var studentService = context.Service<IStudentService>();
                //var students = await studentService.GetStudentsAsync();

                return students;
            });

        //  The preceding code uses the UseFiltering() method to enable filtering on the Students
        //  field. Then, we use the AsQueryable() method to expose the IQueryable interface.
        //  This allows HotChocolate to translate the GraphQL filter to SQL-native queries automatically

        // descriptor.Field(x => x.StudentsWithCustomFilter)
        //     .Description("This is the list of students in the school.")
        //     .UseFiltering<CustomStudentFilterType>()
        //     .Resolve(async context =>
        //     {
        //         var service = context.Service<IStudentService>();
        //
        //         // The following code uses the custom filter.
        //         var filter = context.GetFilterContext()?.ToDictionary();
        //         if (filter != null && filter.ContainsKey("groupId"))
        //         {
        //             var groupFilter = filter["groupId"]! as Dictionary<string, object>;
        //             if (groupFilter != null && groupFilter.ContainsKey("eq"))
        //             {
        //                 if (!Guid.TryParse(groupFilter["eq"].ToString(), out var groupId))
        //                 {
        //                     throw new ArgumentException("Invalid group id", nameof(groupId));
        //                 }
        //
        //                 var students = await service.GetStudentsByGroupIdAsync(groupId);
        //                 return students;
        //             }
        //
        //             if (groupFilter != null && groupFilter.ContainsKey("in"))
        //             {
        //                 if (groupFilter["in"] is not IEnumerable<string> groupIds)
        //                 {
        //                     throw new ArgumentException("Invalid group ids", nameof(groupIds));
        //                 }
        //
        //                 groupIds = groupIds.ToList();
        //                 if (groupIds.Any())
        //                 {
        //                     var students =
        //                         await service.GetStudentsByGroupIdsAsync(groupIds
        //                             .Select(x => Guid.Parse(x.ToString())).ToList());
        //                     return students;
        //                 }
        //                 return new List<Student>();
        //
        //             }
        //         }
        //         var allStudents = await service.GetStudentsAsync();
        //         return allStudents;
        //     });

        descriptor.Field(x => x.StudentsWithCustomFilter)
            .Description("This is the list of students in the school.")
            .UseFiltering<CustomStudentFilterType>()
            .Resolve(async context =>
            {
                var service = context.Service<IStudentService>();

                // The following code uses the custom filter.
                var filter = context.GetFilterContext()?.ToDictionary();
                if (filter != null && filter.ContainsKey("groupId"))
                {
                    var groupFilter = filter["groupId"]! as Dictionary<string, object>;
                    if (groupFilter != null && groupFilter.ContainsKey("eq"))
                    {
                        if (!Guid.TryParse(groupFilter["eq"].ToString(), out var groupId))
                            throw new ArgumentException("Invalid group id", nameof(groupId));

                        var students = await service.GetStudentsByGroupIdAsync(groupId);
                        return students;
                    }

                    if (groupFilter != null && groupFilter.ContainsKey("in"))
                    {
                        if (groupFilter["in"] is not IEnumerable<string> groupIds)
                            throw new ArgumentException("Invalid group ids", nameof(groupIds));

                        groupIds = groupIds.ToList();
                        if (groupIds.Any())
                        {
                            var students =
                                await service.GetStudentsByGroupIdsAsync(groupIds
                                    .Select(x => Guid.Parse(x.ToString())).ToList());
                            return students;
                        }

                        return new List<Student>();
                    }
                }

                var allStudents = await service.GetStudentsAsync();
                return allStudents;
            });

        //  The preceding code is a bit complex. We need to get the filter from the context object. Then,
        //  we check whether the filter contains the groupId property. If the filter contains the groupId
        //  property, we check whether the eq or in operation is specified. If the eq operation is specified,
        //  we retrieve the list of Student objects by the group ID. If the in operation is specified, we
        //  retrieve the list of Student objects by the list of group IDs. If the eq or in operation is not
        //  specified, we retrieve all the Student objects.
    }
    //  The preceding code defines the root query type. In this query type, we specify the type of
    //  the field to be TeacherType. Next, we use the Argument() method to define the id
    //  argument, which is a non-nullable UUID type. Then, we use the Resolve() method to define
    //  the resolver. The resolver takes the id argument and retrieves the Teacher object from the
    //  database. Note that AppDbContext is injected into the resolver from the context object.
}