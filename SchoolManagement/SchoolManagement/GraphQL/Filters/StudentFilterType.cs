using HotChocolate.Data.Filters;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Filters;

public class StudentFilterType : FilterInputType<Student>
{
    protected override void
        Configure(IFilterInputTypeDescriptor<Student> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(t => t.Id);
        descriptor.Field(t => t.GroupId);
        descriptor.Field(t => t.FirstName).Type<StudentStringOperationFilterInputType>();
        descriptor.Field(t => t.LastName).Type<StudentStringOperationFilterInputType>();
        descriptor.Field(t => t.DateOfBirth);
        // descriptor.Field(t => t.FirstName).Type<StudentStringOperationFilterInputType>();
        // descriptor.Field(t => t.LastName).Type<StudentStringOperationFilterInputType>();
    }

    // The following configuration uses the BindFieldsImplicitly method to bind all the fields of the Student class to the StudentFilterType class
    // and explicitly ignores some properties.
    //override protected void Configure(IFilterInputTypeDescriptor<Student> descriptor)
    //{
    //    descriptor.BindFieldsImplicitly();
    //    descriptor.Ignore(t => t.Group);
    //    descriptor.Ignore(t => t.Courses);
    //}
    //  In the preceding code, all the properties of the Student object type will be included in the
    //  StudentFilterInput filter except the Group and Courses properties.

    //  By default, StringOperationFilterInput includes many operations, such as eq, neq,
    //  in, nin, contains, notContains, startsWith, and endsWith. If we do not want
    //  to include all these operations, we can specify the operations by using a custom operation
    //  filter. For example, we can define a StudentStringOperationFilterInputType
    //  class as follows:
}

public class StudentStringOperationFilterInputType : StringOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Operation(DefaultFilterOperations.Equals).Type<StringType>();
        descriptor.Operation(DefaultFilterOperations.Contains).Type<StringType>();
    }
}

public class CustomStudentFilterType : FilterInputType<Student>
{
    protected override void Configure(IFilterInputTypeDescriptor<Student> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Name("CustomStudentFilterInput");
        descriptor.AllowAnd(false).AllowOr(false);
        descriptor.Field(t => t.GroupId).Type<CustomStudentGuidOperationFilterInputType>();
    }
}

public class CustomStudentGuidOperationFilterInputType : UuidOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name("CustomStudentGuidOperationFilterInput");
        descriptor.Operation(DefaultFilterOperations.Equals).Type<IdType>();
        descriptor.Operation(DefaultFilterOperations.In).Type<ListType<IdType>>();
    }
}
//  In the preceding code, we define a custom filter input type named
//  CustomStudentFilterInput. The CustomStudentFilterInput
//  filter only includes the GroupId property. To make the filter more simple,
//  we disable the and and or operations. Then, we define a custom filter input 
//  type named CustomStudentGuidOperationFilterInput. The
//  CustomStudentGuidOperationFilterInput filter only includes the eq
//  and in operations. Note that we need to specify the names of the filter input types.
//  Otherwise, HotChocolate will report name conflicts because we already have a
//  StudentFilterInput filter.