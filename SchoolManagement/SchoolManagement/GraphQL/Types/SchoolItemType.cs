using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Types;

public class SchoolItemType : UnionType
{
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        descriptor.Name("SchoolItem");
        descriptor.Type<EquipmentType>();
        descriptor.Type<FurnitureType>();
    }
}

public class EquipmentType : ObjectType<Equipment>
{
    protected override void Configure(IObjectTypeDescriptor<Equipment> descriptor)
    {
        base.Configure(descriptor);
        descriptor.Name("Equipment");
    }
}

public class FurnitureType : ObjectType<Furniture>
{
    protected override void Configure(IObjectTypeDescriptor<Furniture> descriptor)
    {
        base.Configure(descriptor);
        descriptor.Name("Furniture");
    }
}

//  The preceding code defines a union type named SchoolItem. A union type must inherit
//  from the UnionType class. Then, we use the Type method to specify the object types
//  that are included in the union type. In this case, the SchoolItem union type includes the
//  EquipmentType and FurnitureType object types.