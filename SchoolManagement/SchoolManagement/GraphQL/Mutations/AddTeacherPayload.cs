using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Mutations;

// public class AddTeacherPayload
// {
//     public AddTeacherPayload(Teacher teacher)
//     {
//         Teacher = teacher;
//     }
//
//     public Teacher Teacher { get; }
// }

public class AddTeacherPayload(Teacher teacher)
{
    public Teacher Teacher { get; } = teacher;
}